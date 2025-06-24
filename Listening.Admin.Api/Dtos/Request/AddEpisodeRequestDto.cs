namespace Listening.Admin.Api.Dtos.Request
{
    public class AddEpisodeRequestDto
    {
        public long AlumId { get; set; }
        public string Title { get; set; }
        public string SubtitleType { get; set; }
        public string SubtitleContent { get; set; }
        public Uri AudioUrl { get; set; }
        public long DurationInSeconds { get; set; }
        public Uri? CoverImgUrl { get; set; }

    }
}
