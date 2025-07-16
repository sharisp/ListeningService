using Listening.Domain.Entities;

namespace Listening.Admin.Api.Dtos.Response
{
    public class EpisodeResponseDto
    {
        public long Id { get; set; }
        public long AlbumId { get; set; }
        public string SubtitleContent { get; set; }
        public Uri AudioUrl { get; set; }
        public long DurationInSeconds { get; set; }

        public int SequenceNumber { get; set; }
        public string Title { get; set; }
        public Uri? CoverImgUrl { get; set; }
        public bool IsShow { get; set; }

        public static EpisodeResponseDto ToDto(Episode episode, bool subtitleContentEmputy)
        {
            return new EpisodeResponseDto
            {
                Id = episode.Id,
                Title = episode.Title,
                AlbumId = episode.AlbumId,
                SubtitleContent = subtitleContentEmputy ? "" : episode.SubtitleContent,
                AudioUrl = episode.AudioUrl,
                DurationInSeconds = episode.DurationInSeconds,
                SequenceNumber = episode.SequenceNumber,
                CoverImgUrl = episode.CoverImgUrl,
                IsShow = episode.IsShow
            };

        }

    }
}
