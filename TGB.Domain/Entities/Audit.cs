using System.ComponentModel.DataAnnotations;

namespace TGB.Domain.Entities
{
    public class Audit
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public Guid TypeId { get; set; }

        public string User { get; set; }
        public DateTimeOffset Time { get; set; }
        public string Action { get; set; }
    }
}
