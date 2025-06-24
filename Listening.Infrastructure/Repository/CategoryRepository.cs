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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(Category model)
        {
           this.dbContext.Categories.Add(model);
        }

        public async Task<List<Category>> GetAllByKindIdAsync(long kindId)
        {

          return await  this.dbContext.Categories.Where(t=>t.KindId==kindId).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(long id)
        {

            return await this.dbContext.Categories.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> GetMaxSequenceNumberAsync(long kindId)
        {

            return await dbContext.Categories.Where(t => t.KindId == kindId).MaxAsync(t => (int?)t.SequenceNumber) ?? 0;
        }
    }
}
