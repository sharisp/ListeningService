using Listening.Domain.Entities;

namespace Listening.Api.Dtos
{
    public class EpisodeResponseDto
    {

        public long Id { get; set; }
        public string Title { get; set; }
        public long AlbumId { get; set; }
        public Uri? CoverImgUrl { get; set; }
        public string SubtitleContent { get;  set; }
        public Uri AudioUrl { get;  set; }
        public long DurationInSeconds { get;  set; }

    }
}
