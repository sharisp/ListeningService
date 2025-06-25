using IdGen;
using Listening.Api.Dtos;
using Listening.Api.Dtos.Mapper;
using Listening.Api.Helpers;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Api.Controllers
{

    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KindController(
           IKindRepository repository, MemoryCacheHelper memoryCacheHelper, KindMapper mapper) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<KindResponseDto?>>> FindById(long id)
        {
            var kind = await memoryCacheHelper.GetOrCreateAsync<Kind?>($"KindController_FindById_{id}", async entry =>
                {

                    return await repository.GetByIdAsync(id);
                });
            var responseDto = mapper.ToDto(kind);
            return Ok(ApiResponse<KindResponseDto?>.Ok(responseDto));
        }

        [HttpGet("List")]
        public async Task<ActionResult<ApiResponse<List<KindResponseDto>>>> GetAll()
        {
            var kinds = await memoryCacheHelper.GetOrCreateAsync<List<Kind>>($"KindController_GetAll", async entry =>
                        {

                            return await repository.GetAllAsync();
                        });

            var responseDtos = mapper.ToDtos(kinds);
            return Ok(ApiResponse<List<KindResponseDto>>.Ok(responseDtos));
        }

    }
}
