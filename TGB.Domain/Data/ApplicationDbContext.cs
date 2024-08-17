using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TGB.Domain.Entities;

namespace TGB.Domain.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
	{
		public DbSet<GroupBank> GroupBanks { get; set; }
		public DbSet<Record> Records { get; set; }
		public DbSet<Audit> Audits { get; set; }
    }
}
