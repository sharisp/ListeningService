using Listening.Domain.Entities;

namespace Listening.Api.Dtos
{
    public class CategoryResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long KindId { get; set; }
        public Uri? CoverImgUrl { get; set; }

    }
}
