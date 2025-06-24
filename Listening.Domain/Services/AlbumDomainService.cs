using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listening.Domain.Entities;
using Listening.Domain.Helper;
using Listening.Domain.Interfaces;

namespace Listening.Domain.Services
{
    public class AlbumDomainService(IAlbumRepository repository)
    {

        public async Task<Album> AddAsync(string title, long categoryId, Uri? coverImgUrl)
        {

            var sequenceNumber = await repository.GetMaxSequenceNumberAsync(categoryId);
            var model = new Album(title, categoryId, sequenceNumber + 1, coverImgUrl);
            repository.Add(model);
            return model;
        }
        public async Task SortAsync(long categoryId,List<long> albumIds)
        {
            var albums = await repository.GetAllByCategoryIdAsync(categoryId);

            var idsInDB = albums.Select(a => a.Id);
            if (!idsInDB.SequenceIgnoredEqual(albumIds))
            {
                throw new InvalidEnumArgumentException("The provided album IDs do not match the IDs in the database.");
            }
            int sequenceNumber = 1;
            foreach (var albumId in albumIds)
            {
                var model = await repository.GetByIdAsync(albumId);
                model.ChangeSequenceNumber(sequenceNumber);
                sequenceNumber++;
            }
        }
    }
}
