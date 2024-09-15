using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using TGB.Domain.Data;
using TGB.Domain.Entities;

namespace TGB.Domain.Services
{
    public class Service
    {
        private ILogger _logger;
        private ApplicationDbContext _context;

        public Service(
            ILogger logger
            , IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _context = dbFactory.CreateDbContext();

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

        // NOTES

        public async Task<List<Note>> GetNotesForMenu(string groupBankName)
        {
            return await _context.Notes
                .Where(n => n.GroupBank.Name == groupBankName)
                .Select(n => new Note
                {
                    Id = n.Id,
                    Title = n.Title,
                    Tags = n.Tags
                })
                .ToListAsync();
        }

        public async Task<Note> CreateNote(Note note, Guid groupBankId)
        {
            var bank = await _context.GroupBanks
                .Where(gb => gb.Id == groupBankId)
                .FirstOrDefaultAsync();

            if (bank == null)
            {
                throw new Exception("Group bank not found");
            }

            note.GroupBank = bank;
            _context.Notes.Add(note);

            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateNote(Note note, Guid groupBankId)
        {
            var local = _context.Set<Note>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(note.Id));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task DeleteNote(Guid noteId)
        {
            Note note = await _context.Notes
                .Where(n => n.Id == noteId)
                .FirstOrDefaultAsync();

            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Note> GetNote(Guid noteId)
        {
            return await _context.Notes
                .Where(n => n.Id == noteId)
                .FirstOrDefaultAsync();
        }

        // IDENTITY

        public async Task<List<IdentityUser>> GetUsersForBank(Guid bankId)
        {

            var userIds = await _context.UserClaims
                 .Where(uc => uc.ClaimType == TgbClaimTypes.GroupBankUserClaim && uc.ClaimValue == bankId.ToString())
                 .Select(uc => uc.UserId)
                 .ToListAsync();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new IdentityUser
                {
                    UserName = u.UserName,
                    Id = u.Id
                })
                .ToListAsync();

            return users;
        }

        
    }
}
