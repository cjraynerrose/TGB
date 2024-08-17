using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using TGB.Domain.Data;
using TGB.Domain.Entities;

namespace TGB.Domain
{
    public class Service
    {
        private ILogger _logger;
        private ApplicationDbContext _context;

        public Service(
            ILogger logger
            , ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<GroupBank> CreateGroupBank(string name, List<string> userIds)
        {
            GroupBank groupBank = new GroupBank
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            _context.GroupBanks.Add(groupBank);

            _context.UserClaims.AddRange(userIds.Select(userId => new IdentityUserClaim<string>
            {
                UserId = userId,
                ClaimType = TgbClaimTypes.GroupBankUserClaim,
                ClaimValue = groupBank.Id.ToString()
            }));

            await _context.SaveChangesAsync();

            return groupBank;
        }

        public async Task AddUserToGroupBank(string userId, Guid groupBankId)
        {
            _context.UserClaims.Add(new IdentityUserClaim<string>
            {
                UserId = userId,
                ClaimType = TgbClaimTypes.GroupBankUserClaim,
                ClaimValue = groupBankId.ToString()
            });

            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromGroupBank(string userId, Guid groupBankId)
        {
            IdentityUserClaim<string> claim = await _context.UserClaims
                .Where(uc => uc.UserId == userId && uc.ClaimType == TgbClaimTypes.GroupBankUserClaim && uc.ClaimValue == groupBankId.ToString())
                .FirstOrDefaultAsync();

            if (claim != null)
            {
                _context.UserClaims.Remove(claim);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<GroupBank>> GetGroupBanksForUser(string userId)
        {
            return await _context.UserClaims
                .Join(
                    _context.GroupBanks,
                    userClaims => userClaims.ClaimValue,
                    groupBanks => groupBanks.Id.ToString(),
                    (uc, gb) => new
                    {
                        gb.Id,
                        gb.Name,
                        uc.UserId,
                        uc.ClaimType
                    })
                .Where(uc => uc.UserId == userId && uc.ClaimType == TgbClaimTypes.GroupBankUserClaim)
                .Select(uc => new GroupBank
                {
                    Id = uc.Id,
                    Name = uc.Name
                })
                .ToListAsync();
        }

        public async Task<List<GroupBank>> GetGroupBanksForUser(ClaimsPrincipal principal)
        {
            var groupBankIds = principal.FindAll(TgbClaimTypes.GroupBankUserClaim).Select(c => Guid.Parse(c.Value)).ToList();
            return await _context.GroupBanks
                .Where(gb => groupBankIds.Contains(gb.Id))
                .Select(gb => new GroupBank
                {
                    Id = gb.Id,
                    Name = gb.Name
                })
                .ToListAsync();
        }

        public async Task<GroupBank> GetGroupBank(Guid groupBankId)
        {
            return await _context.GroupBanks
                .Include(gb => gb.Records)
                .Where(gb => gb.Id == groupBankId)
                .FirstOrDefaultAsync();
        }

        public async Task<GroupBank> UpdateGroupBank(GroupBank groupBank)
        {
            var local = _context.Set<GroupBank>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(groupBank.Id));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
                foreach (var record in local.Records)
                {
                    _context.Entry(record).State = EntityState.Detached;
                }
            }

            _context.Entry(groupBank).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return groupBank;
        }

        public async Task<Record> CreateRecord(Record record, Guid GroupBankId)
        {
            var bank = await _context.GroupBanks
                .Where(gb => gb.Id == GroupBankId)
                .FirstOrDefaultAsync();

            if (bank == null)
            {
                throw new Exception("Group bank not found");
            }

            bank.Records.Add(record);

            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<Record> UpdateRecord(Record record)
        {
            var local = _context.Set<Record>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(record.Id));

            if (local != null) 
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            
            _context.Entry(record).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task DeleteRecord(Guid recordId)
        {
            Record record = await _context.Records
                .Where(r => r.Id == recordId)
                .FirstOrDefaultAsync();

            if (record != null)
            {
                _context.Records.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Record> GetRecord(Guid recordId)
        {
            return await _context.Records
                .Where(r => r.Id == recordId)
                .FirstOrDefaultAsync();
        }
    }
}
