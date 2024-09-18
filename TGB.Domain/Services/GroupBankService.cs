using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using TGB.Domain.Data;
using TGB.Domain.Entities;

namespace TGB.Domain.Services
{
    public class GroupBankService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public GroupBankService(
            ILogger logger
            , IDbContextFactory<ApplicationDbContext> dbFactory
            )
        {
            _logger = logger;
            _context = dbFactory.CreateDbContext();
        }

        public async Task<Campaign> CreateCampaign(string name, List<string> userIds)
        {
            GroupBank groupBank = new GroupBank
            {
                Id = Guid.NewGuid()
            };
            
            Campaign campaign = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                GroupBank = groupBank
            };


            _context.Campaigns.Add(campaign);

            _context.GroupBanks.Add(groupBank);

            _context.UserClaims.AddRange(userIds.Select(userId => new IdentityUserClaim<string>
            {
                UserId = userId,
                ClaimType = TgbClaimTypes.CampaignUserClaim,
                ClaimValue = campaign.Id.ToString()
            }));

            await _context.SaveChangesAsync();

            return campaign;
        }

        public async Task AddUserToCampaign(string userId, Guid campaignId)
        {
            _context.UserClaims.Add(new IdentityUserClaim<string>
            {
                UserId = userId,
                ClaimType = TgbClaimTypes.CampaignUserClaim,
                ClaimValue = campaignId.ToString()
            });

            await _context.SaveChangesAsync();
        }

        public async Task<Result> RemoveUserFromCampaign(string userId, Guid campaignId)
        {
            IdentityUserClaim<string> claim = await _context.UserClaims
                .Where(uc => uc.UserId == userId && uc.ClaimType == TgbClaimTypes.CampaignUserClaim && uc.ClaimValue == campaignId.ToString())
                .FirstOrDefaultAsync();

            if (claim != null)
            {
                _context.UserClaims.Remove(claim);
                await _context.SaveChangesAsync();
            }

            return new Result
            {
                Success = claim != null,
                Message = claim != null ? "User removed successfully" : "User not found"
            };
        }

        public async Task<List<Campaign>> GetCampaignsForUser(string userId)
        {

            return await _context.UserClaims
                .Join(
                    _context.Campaigns,
                    userClaims => userClaims.ClaimValue,
                    campaigns => campaigns.Id.ToString(),
                    (uc, cp) => new
                    {
                        cp.Id,
                        cp.Name,
                        uc.UserId,
                        uc.ClaimType,
                        GroupBankId = cp.GroupBank.Id,
                        NoteIds = cp.Notes.Select(n => n.Id)
                    })
                .Where(uc => uc.UserId == userId && uc.ClaimType == TgbClaimTypes.CampaignUserClaim)
                .Select(uc => new Campaign
                {
                    Id = uc.Id,
                    Name = uc.Name,
                    GroupBank = new() { Id = uc.GroupBankId },
                    Notes = uc.NoteIds.Select(nid => new Note() { Id = nid }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetCampaignsForUser(ClaimsPrincipal principal)
        {
            var userId = principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (userId == null)
            {
                return [];
            }

            return await GetCampaignsForUser(userId);
        }

        public async Task<GroupBank> GetGroupBank(Guid groupBankId)
        {
            return await _context.GroupBanks
                .Include(gb => gb.Records)
                .Where(gb => gb.Id == groupBankId)
                .FirstOrDefaultAsync();
        }


        public async Task<Campaign> GetCampaign(Guid campaignId)
        {
            return await _context.Campaigns
                .Include(c => c.GroupBank.Records)
                .Where(gb => gb.Id == campaignId)
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> GetCampaignIdFromName(string name)
        {
            return await _context.Campaigns
                .Where(gb => gb.Name == name)
                .Select(gb => gb.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Result<IdentityUser>> TryAddEmailToCampaign(string email, Guid CampaignId)
        {
            var user = await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new Result<IdentityUser>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            await AddUserToCampaign(user.Id, CampaignId);

            return new Result<IdentityUser>
            {
                Success = true,
                Message = "User added successfully",
                Data = new IdentityUser
                {
                    UserName = user.UserName,
                    Id = user.Id
                }
            };
        }
    }
}
