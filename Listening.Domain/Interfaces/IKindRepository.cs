using Listening.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.Interfaces
{
    public interface IKindRepository
    {
        IQueryable<Kind> Query();
        void Add(Kind model);
        Task<List<Kind>> GetAllAsync();
        Task<Kind?> GetByIdAsync(long id);
        Task<int> GetMaxSequenceNumberAsync();
    }
}
