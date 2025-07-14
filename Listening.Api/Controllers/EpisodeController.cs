using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Listening.Api.Dtos;
using Listening.Api.Dtos.Mapper;
using Listening.Api.Helpers;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Listening.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Api.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeController(
           IEpisodeRepository repository, MemoryCacheHelper memoryCacheHelper, EpisodeMapper mapper) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EpisodeResponseDto?>>> FindById(long id)
        {
            var info = await memoryCacheHelper.GetOrCreateAsync<EpisodeResponseDto>($"EpisodeController_FindById_"+ id, async entry =>
            {

                var episode= await repository.GetByIdAsync(id);
                return mapper.ToDto(episode);
            });
            return Ok(ApiResponse<EpisodeResponseDto?>.Ok(info));
        }

        [HttpGet("ListByAlbum/{albumId}")]
        public async Task<ActionResult<ApiResponse<List<EpisodeResponseDto>>>> FindByAlbumId(long albumId)
        {
            var info = await memoryCacheHelper.GetOrCreateAsync<List<EpisodeResponseDto>>($"EpisodeController_FindByAlbumId_"+ albumId, async entry =>
            {

                var episodes = await repository.GetAllByAlumIdAsync(albumId);
                return mapper.ToDtos(episodes.Where(t=>t.IsShow==true).ToList());
            });
            return Ok(ApiResponse<List<EpisodeResponseDto>>.Ok(info));
        }

    }
}
