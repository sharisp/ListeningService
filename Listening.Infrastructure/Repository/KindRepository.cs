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
    public class KindRepository : IKindRepository
    {
        private readonly AppDbContext dbContext;

        public KindRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<Kind> Query()
        {
            var query = dbContext.Kinds.AsQueryable();

            return query;
        }
        public void Add(Kind model)
        {

            dbContext.Kinds.Add(model);
        }

        public async Task<List<Kind>> GetAllAsync()
        {
            return await dbContext.Kinds.OrderBy(t => t.SequenceNumber).ToListAsync();
        }

        public async Task<Kind?> GetByIdAsync(long id)
        {
            return await dbContext.Kinds.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> GetMaxSequenceNumberAsync()
        {

            return await dbContext.Kinds.MaxAsync(t => (int?)t.SequenceNumber)??0;
        }
    }
}