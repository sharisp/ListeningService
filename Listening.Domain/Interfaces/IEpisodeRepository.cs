using Listening.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.Interfaces
{
    public interface IEpisodeRepository
    {
        IQueryable<Episode> Query();
        void Add(Episode model);
        Task<List<Episode>> GetAllByAlumIdAsync(long albumId);
        Task<Episode?> GetByIdAsync(long id);
        Task<int> GetMaxSequenceNumberAsync(long albumId);
    }
}
