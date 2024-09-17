using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TGB.Domain.Entities
{
    public class Npc
    {
        [Key]
        public Guid Id { get; set; }
        //TODO Link to campaign later 
        public string Name { get; set; }
        public List<NpcAttribites> Attributes { get; set; } = [];

    }
    public class NpcAttribites
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
