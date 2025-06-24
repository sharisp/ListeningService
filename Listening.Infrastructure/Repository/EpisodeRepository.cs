using IdGen;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure.Repository
{
    public class EpisodeRepository : IEpisodeRepository
    {
        private readonly AppDbContext dbContext;

        public EpisodeRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(Episode model)
        {
           dbContext.Episodes.Add(model);
        }

        public async Task<List<Episode>> GetAllByAlumIdAsync(long albumId)
        {

            return await dbContext.Episodes.Where(t => t.AlbumId == albumId).ToListAsync();
        }

        public async Task<Episode?> GetByIdAsync(long id)
        {
            return await dbContext.Episodes.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> GetMaxSequenceNumberAsync(long albumId)
        {

            return await dbContext.Episodes.Where(t => t.AlbumId == albumId).MaxAsync(t => (int?)t.SequenceNumber) ?? 0;
        }
    }
}
