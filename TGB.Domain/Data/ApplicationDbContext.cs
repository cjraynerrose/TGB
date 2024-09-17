using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;
using System.Text.Json;
using TGB.Domain.Entities;

namespace TGB.Domain.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(
			DbContextOptions<ApplicationDbContext> options
			, IHttpContextAccessor httpContextAccessor)
		: base(options)
        {
			_httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Campaign> Campaigns { get; set; }
		public DbSet<GroupBank> GroupBanks { get; set; }
		public DbSet<Record> Records { get; set; }
		public DbSet<Audit> Audits { get; set; }
        public DbSet<Npc> Npcs { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder
                .Properties<Guid>()
                .HaveConversion<GuidToTextConverter>();
        }



        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
			if(!ChangeTracker.HasChanges())
            {
				return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
			}

			List<Audit> audits = [];

			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry is null)
                {
                    continue;
                }

				if(entry.Entity.GetType() == typeof(Audit))
				{
					continue;
                }

				if(entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }


				Guid id = Guid.Empty;
				var idProp = entry.Entity.GetType().GetProperty("Id");
				if (idProp != null)
				{
					var idType = idProp?.PropertyType;
					if (idType == typeof(Guid))
					{
						id = (Guid)idProp.GetValue(entry.Entity);
					}
				}


                Audit audit = new()
                {
                    Action = entry.State switch
                    {
                        EntityState.Added => AuditAction.Create,
                        EntityState.Modified => AuditAction.Update,
                        EntityState.Deleted => AuditAction.Delete,

                        _ => throw new NotImplementedException()
                    },
                    TypeId = id,
					TypeName = entry.Entity.GetType().Name ?? "UNKNOWN_TYPE",
                    OldValues = entry.State == EntityState.Modified || entry.State == EntityState.Deleted ? JsonSerializer.Serialize(entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p])) : null,
                    NewValues = entry.State == EntityState.Modified || entry.State == EntityState.Added ? JsonSerializer.Serialize(entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p])) : null,
					UserId = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? "UNKNOWN_USER"
                };

                audits.Add(audit);
			}

			foreach (var audit in audits)
			{
                Entry(audit).State = EntityState.Added;
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
