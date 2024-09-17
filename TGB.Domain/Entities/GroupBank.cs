using System.ComponentModel.DataAnnotations;

namespace TGB.Domain.Entities
{
    public class GroupBank
    {
        [Key]
        public Guid Id { get; set; }

        public virtual List<Record> Records { get; set; } = [];
    }
}
