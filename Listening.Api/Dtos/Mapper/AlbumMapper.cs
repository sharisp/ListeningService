using Listening.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Listening.Api.Dtos.Mapper
{
    [Mapper]
    public partial class AlbumMapper : IMapperService
    {
        public partial AlbumResponseDto? ToDto(Album? info);
        public partial List<AlbumResponseDto> ToDtos(List<Album> infos);
    }
}
