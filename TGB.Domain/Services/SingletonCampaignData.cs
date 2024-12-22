using Microsoft.EntityFrameworkCore;
using Serilog;
using TGB.Domain.Data;
using TGB.Domain.Entities;

namespace TGB.Domain.Services
{
    public class SingletonCampaignData : ISingletonCampaignData
    {
        private readonly Guid _campaignId;

        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public event EventHandler<ChangeEvent>? DataUpdated;

        public SingletonCampaignData(
            Guid campaignId
            , ILogger logger
            , ApplicationDbContext context
            )
        {
            _campaignId = campaignId;
            _logger = logger;
            _context = context;
        }

        public async Task CreateRecord(Record record, Guid GroupBankId)
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
            DataUpdated?.Invoke(this, new(record, ChangeType.Added));
        }

        public async Task UpdateRecord(Record record)
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
            DataUpdated?.Invoke(this, new(record, ChangeType.Updated));
        }

        public async Task DeleteRecord(Guid recordId)
        {
            Record record = await _context.Records
                .Where(r => r.Id == recordId)
                .SingleAsync();

            if (record == null)
            {
                return;
            }

            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
            DataUpdated?.Invoke(this, new(record, ChangeType.Updated));
        }

        public async Task<Record> GetRecord(Guid recordId)
        {
            return await _context.Records
                .Where(r => r.Id == recordId)
                .SingleAsync();
        }

        public async Task<List<Record>> GetRecords(Page page, Filter<Record> filter)
        {
            var data = await _context.Records
                .Where(c => filter.Predicate(c))
                .Skip(page.PageIndex)
                .Take(page.PageSize)
                .ToListAsync();

            return data;
        }

    }

    public interface ISingletonCampaignData
    {
        Task CreateRecord(Record record, Guid GroupBankId);
        Task DeleteRecord(Guid recordId);
        Task<Record> GetRecord(Guid recordId);
        Task<List<Record>> GetRecords(Page page, Filter<Record> filter);
        Task UpdateRecord(Record record);
    }

    public struct ChangeEvent(Record record, ChangeType changeType)
    {
        public Record Record = record;
        public ChangeType ChangeType = changeType;
    }

    public enum ChangeType
    {
        Added, Updated, Deleted
    }

    public struct Page
    {
        public int PageSize;
        public int PageIndex;
    }

    public struct Filter<T>
    {
        public Func<T, bool> Predicate;
    }
}
