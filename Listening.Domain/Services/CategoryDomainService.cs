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
    public class CategoryDomainService(ICategoryRepository repository)
    {

        public async Task<Category> AddAsync(string title, long kindId, Uri? coverImgUrl)
        {

            var sequenceNumber = await repository.GetMaxSequenceNumberAsync(kindId);
            var model = new Category(title, sequenceNumber + 1, kindId, coverImgUrl);
            repository.Add(model);
            return model;
        }
        public async Task SortAsync(long kindId, List<long> categoryIds)
        {
            var cagtegories = await repository.GetAllByKindIdAsync(kindId);

            var idsInDB = cagtegories.Select(a => a.Id);
            if (!idsInDB.SequenceIgnoredEqual(categoryIds))
            {
                throw new InvalidEnumArgumentException("The provided category IDs do not match the IDs in the database.");
            }
            int sequenceNumber = 1;
            foreach (var categoryId in categoryIds)
            {
                var model = await repository.GetByIdAsync(categoryId);
                model.ChangeSequenceNumber(sequenceNumber);
                sequenceNumber++;
            }
        }
    }
}