using TGB.Domain.Entities;

namespace TGB.Web.Models
{
    public class CampaignVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CampaignVM() { }
        public CampaignVM(Campaign campaign)
        {
            Id = campaign.Id;
            Name = campaign.Name;
        }

        public Campaign ToCampaign()
        {
            return new Campaign
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
