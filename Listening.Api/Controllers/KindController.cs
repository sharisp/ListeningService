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
           IKindRepository repository, MemoryCacheHelper memoryCacheHelper, BaseEntityMapper mapper) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BaseEntityResponseDto?>>> FindById(long id)
        {
            var responseDto = await memoryCacheHelper.GetOrCreateAsync<BaseEntityResponseDto?>($"KindController_FindById_{id}", async entry =>
                {

                    var info = await repository.GetByIdAsync(id);
                    return mapper.ToDto(info);
                });
            return Ok(ApiResponse<BaseEntityResponseDto?>.Ok(responseDto));
        }

        [HttpGet("List")]
        public async Task<ActionResult<ApiResponse<List<BaseEntityResponseDto>>>> GetAll()
        {
            var responseDtos = await memoryCacheHelper.GetOrCreateAsync<List<BaseEntityResponseDto>>($"KindController_GetAll", async entry =>
                        {

                            var kinds = await repository.GetAllAsync();
                            var dtos = new List<BaseEntityResponseDto>();
                            foreach (var item in kinds.Where(t => t.IsShow == true))
                            {
                                dtos.Add(mapper.ToDto(item));
                            }
                            return dtos;
                        });

            return Ok(ApiResponse<List<BaseEntityResponseDto>>.Ok(responseDtos));
        }

    }
}
