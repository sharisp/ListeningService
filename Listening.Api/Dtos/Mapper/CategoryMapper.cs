using Listening.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Listening.Api.Dtos.Mapper
{
   

    [Mapper]
    public partial class CategoryMapper : IMapperService
    {
        public partial CategoryResponseDto? ToDto(Category? info);
        public partial List<CategoryResponseDto> ToDtos(List<Category> infos);
    }
}
