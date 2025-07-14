using Listening.Domain.Entities;

namespace Listening.Api.Dtos
{
    public class BaseEntityResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Uri? CoverImgUrl { get; set; }
      
        public int SequenceNumber { get; set; }       


    }
}
