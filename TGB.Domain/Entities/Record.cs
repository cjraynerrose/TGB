using System.ComponentModel.DataAnnotations;

namespace TGB.Domain.Entities
{
    public class Record
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public List<string> Tags { get; set; } = [];
    }
}
