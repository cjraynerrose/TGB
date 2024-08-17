using System.ComponentModel.DataAnnotations;

namespace TGB.Domain.Entities
{
    public class Audit
    {
        [Key]
        public int Id { get; set; }
        public int Version { get; } = 1;
        public DateTimeOffset Time { get; set; } = DateTimeOffset.Now;
        public AuditAction Action { get; set; }
        public Guid? TypeId { get; set; } = null;
        public string? TypeName { get; set; } = null;
        public string? OldValues { get; set; } = null;
        public string? NewValues { get; set; } = null;
        public string? UserId { get; set; } = null;
    }

    public enum AuditAction
    {
        Create,
        Update,
        Delete
    }
}
