using Listening.Domain.Entities;

namespace Listening.Api.Dtos
{
    public class EpisodeResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long AlbumId { get; set; }
        public Uri? CoverImgUrl { get; set; }
        public string SubtitleContent { get; private set; }
        public Uri AudioUrl { get; private set; }
        public long DurationInSeconds { get; private set; }

    }
}
