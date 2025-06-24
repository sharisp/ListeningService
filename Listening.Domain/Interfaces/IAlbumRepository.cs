using Listening.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.Interfaces
{
    public interface IAlbumRepository
    {
        Task<List<Album>> GetAllByCategoryIdAsync(long categoryId);

        void Add(Album model);
        Task<Album?> GetByIdAsync(long id);
        Task<int> GetMaxSequenceNumberAsync(long categoryId);

    }
}
