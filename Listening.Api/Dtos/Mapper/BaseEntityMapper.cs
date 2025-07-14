using Listening.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Listening.Api.Dtos.Mapper
{
    /// <summary>
    /// mapperly did not support generic type mapping, so we have to use  class to implement the mapping methods.
    /// </summary>

    [Mapper(AllowNullPropertyAssignment = false, ThrowOnPropertyMappingNullMismatch = false)]
    public partial class BaseEntityMapper : IMapperService
    {
        public partial BaseEntityResponseDto?  ToDto(ListeningBaseEntity? info);
        public partial List<BaseEntityResponseDto> ToDtos(List<ListeningBaseEntity> infos);
    }
}
