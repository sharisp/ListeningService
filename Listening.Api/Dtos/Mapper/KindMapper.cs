using Listening.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Listening.Api.Dtos.Mapper
{
    /// <summary>
    /// mapperly did not support generic type mapping, so we have to use  class to implement the mapping methods.
    /// </summary>

    [Mapper(AllowNullPropertyAssignment = false, ThrowOnPropertyMappingNullMismatch = false)]
    public partial class KindMapper : IMapperService
    {
        public partial KindResponseDto?  ToDto(Kind? info);
        public partial List<KindResponseDto> ToDtos(List<Kind> infos);
    }
}
