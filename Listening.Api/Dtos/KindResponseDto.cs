using Listening.Domain.Entities;

namespace Listening.Api.Dtos
{
    public class KindResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Uri? CoverImgUrl { get; set; }

    }
}
