using System.ComponentModel.DataAnnotations;

namespace TGB.Domain.Entities
{
    public class Note
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }
}
