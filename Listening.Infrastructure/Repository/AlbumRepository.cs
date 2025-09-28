using IdGen;
using Infrastructure.SharedKernel;
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
    public class AlbumRepository : IAlbumRepository
    {
        private readonly AppDbContext dbContext;

        public AlbumRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        public void Add(Album model)
        {
            dbContext.Albums.Add(model);
        }

        public async Task<List<Album>> GetAllByCategoryIdAsync(long categoryId)
        {

            return await dbContext.Albums.Where(t => t.CategoryId == categoryId).OrderBy(t=>t.SequenceNumber).ToListAsync();
        }

        public async Task<Album?> GetByIdAsync(long id)
        {

            return await dbContext.Albums.FirstOrDefaultAsync(t => t.Id == id);
            //  return await dbContext.FindAsync<Album>(id);

        }

        public async Task<int> GetMaxSequenceNumberAsync(long categoryId)
        {

           return await dbContext.Albums.Where(t => t.CategoryId == categoryId).MaxAsync(t =>(int?) t.SequenceNumber)??0;
         
        }
    }
}
