using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Listening.Domain.Helper;

namespace Listening.Domain.Services
{
    public class KindDomainService(IKindRepository repository)
    {
        public async Task<Kind> AddAsync(string title,  Uri? coverImgUrl)
        {

            var sequenceNumber = await repository.GetMaxSequenceNumberAsync();
            var model = new Kind(title, sequenceNumber + 1, coverImgUrl);
            repository.Add(model);
            return model;
        }
        public async Task SortAsync(List<long> kingIds)
        {
            var albums = await repository.GetAllAsync();

            var idsInDB = albums.Select(a => a.Id);
            if (!idsInDB.SequenceIgnoredEqual(kingIds))
            {
                throw new InvalidEnumArgumentException("The provided kind IDs do not match the IDs in the database.");
            }
            int sequenceNumber = 1;
            foreach (var kindId in kingIds)
            {
                var model = await repository.GetByIdAsync(kindId);
                model.ChangeSequenceNumber(sequenceNumber);
                sequenceNumber++;
            }
        }
    }
}
