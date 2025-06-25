using Listening.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Listening.Api.Dtos.Mapper
{

    [Mapper]
    public partial class EpisodeMapper : IMapperService
    {
        public partial EpisodeResponseDto? ToDto(Episode? info);
        public partial List<EpisodeResponseDto> ToDtos(List<Episode> infos);
    }
}
