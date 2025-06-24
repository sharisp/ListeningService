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
    public class EpisodeDomainService(IEpisodeRepository repository)
    {

        public async Task<Episode> AddAsync(long alumId, string title, string subtitleType, string subtitleContent, Uri audioUrl, long durationInSeconds, Uri? coverImgUrl)
        {

            var sequenceNumber = await repository.GetMaxSequenceNumberAsync(alumId);
            var model = new Episode(alumId, title, subtitleType, subtitleContent, audioUrl, durationInSeconds, sequenceNumber, coverImgUrl);
            repository.Add(model);
            return model;
        }
        public async Task SortAsync(long albumId, List<long> episodeIds)
        {
            var episodes = await repository.GetAllByAlumIdAsync(albumId);

            var idsInDB = episodes.Select(a => a.Id);
            if (!idsInDB.SequenceIgnoredEqual(episodeIds))
            {
                throw new InvalidEnumArgumentException("The provided episode IDs do not match the IDs in the database.");
            }
            int sequenceNumber = 1;
            foreach (var episodeId in episodeIds)
            {
                var model = await repository.GetByIdAsync(episodeId);
                model.ChangeSequenceNumber(sequenceNumber);
                sequenceNumber++;
            }
        }
    }
}