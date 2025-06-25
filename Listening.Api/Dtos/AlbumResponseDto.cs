namespace Listening.Api.Dtos
{
    public class AlbumResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long CategoryId { get; set; }
        public Uri? CoverImgUrl { get; set; }
    }
}
