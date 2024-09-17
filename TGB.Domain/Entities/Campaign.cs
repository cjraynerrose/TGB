using System.ComponentModel.DataAnnotations;

namespace TGB.Domain.Entities
{
    public class Campaign
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual GroupBank GroupBank { get; set; }
        public virtual List<Note> Notes { get; set; }
        public virtual List<Npc> Npcs { get; set; }
    }
}
