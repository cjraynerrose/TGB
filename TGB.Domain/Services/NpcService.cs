using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGB.Domain.Data;
using TGB.Domain.Entities;

namespace TGB.Domain.Services
{
    public class NpcService
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public NpcService(
            ILogger logger, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _context = dbFactory.CreateDbContext();
        }

        public async Task<Npc> CreateNpc(string name)
        {
            Npc npc = new()
            {
                Id = Guid.NewGuid(),
                Name = name
            };
            _context.Npcs.Add(npc);

            await _context.SaveChangesAsync();

            return npc;
        }
        public async Task RemoveNpc(Guid npcId)
        {

            Npc npc = await _context.Npcs.Where(npc => npc.Id == npcId).FirstOrDefaultAsync();
            if (npc != null)
            {
                _context.Npcs.Remove(npc);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogTrace("NPC {npcId} not found, should be fine but could not find it.", npcId);
            }
        }
    }
}
