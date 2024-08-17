using TGB.Domain.Entities;

namespace TGB.Web.Models
{
    public class GroupBankVM
    {
        public GroupBankVM() { }
        public GroupBankVM(GroupBank groupBank)
        {
            Id = groupBank.Id;
            Name = groupBank.Name;
            Records = groupBank.Records.Select(r => new RecordVM(r)).ToList();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RecordVM> Records { get; set; } = [];

        public GroupBank ToGroupBank()
        {
            return new GroupBank
            {
                Id = Id,
                Name = Name,
                Records = Records.Select(r => r.ToRecord()).ToList()
            };
        }
    }
}
