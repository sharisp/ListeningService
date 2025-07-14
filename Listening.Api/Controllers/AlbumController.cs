using Domain.SharedKernel.Interfaces;
using FluentValidation;
using IdGen;
using Listening.Api.Dtos;
using Listening.Api.Dtos.Mapper;
using Listening.Api.Helpers;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Listening.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Api.Controllers
{
    //  [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController(
            IAlbumRepository repository, MemoryCacheHelper memoryCacheHelper, BaseEntityMapper mapper) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BaseEntityResponseDto?>>> FindById(long id)
        {
            var album = await memoryCacheHelper.GetOrCreateAsync<BaseEntityResponseDto?>($"AlbumController_FindById_"+ id, async entry =>
            {
                var info = await repository.GetByIdAsync(id);
                return mapper.ToDto(info);
            });
            return Ok(ApiResponse<BaseEntityResponseDto?>.Ok(album));
        }

        [HttpGet("ListByCatagory/{categoryId}")]
        public async Task<ActionResult<ApiResponse<List<BaseEntityResponseDto>?>>> FindByCategoryId(long categoryId)
        {
            var responseDtos = await memoryCacheHelper.GetOrCreateAsync<List<BaseEntityResponseDto>?>($"AlbumController_FindByCategoryId_"+categoryId, async entry =>
            {

                var albums = await repository.GetAllByCategoryIdAsync(categoryId);
                var dtos = new List<BaseEntityResponseDto>();
                foreach (var item in albums.Where(t => t.IsShow == true))
                {
                    dtos.Add(mapper.ToDto(item));
                }
                return dtos;
            });

            return Ok(ApiResponse<List<BaseEntityResponseDto>>.Ok(responseDtos));
        }


    }
}
