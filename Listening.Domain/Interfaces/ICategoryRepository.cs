using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listening.Domain.Entities;

namespace Listening.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllByKindIdAsync(long kindId);
        Task<Category?> GetByIdAsync(long id);
        Task<int> GetMaxSequenceNumberAsync(long kindId);

        void Add(Category model);

    }
}
