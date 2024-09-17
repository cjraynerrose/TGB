using TGB.Domain.Entities;

namespace TGB.Web.Models
{
    public class NpcVM
    {
        public NpcVM(Npc npc) {
            Id = npc.Id;
            Name = npc.Name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = [];
    }
}
