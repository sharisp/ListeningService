using Domain.SharedKernel.Interfaces;
using FluentValidation;
using IdGen;
using Listening.Api.Dtos;
using Listening.Api.Dtos.Mapper;
using Listening.Api.Helpers;
using Listening.Domain.Entities;
using Listening.Domain.Helper;
using Listening.Domain.Interfaces;
using Listening.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Listening.Api.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeController(
           IEpisodeRepository repository, MemoryCacheHelper memoryCacheHelper) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EpisodeResponseDto?>>> FindById(long id)
        {
          

            var info = await memoryCacheHelper.GetOrCreateAsync<EpisodeResponseDto>($"EpisodeController_FindById_"+ id, 
                async entry =>
            {
                var episode =await repository.Query().Where(t => t.Id == id).Select(t => new EpisodeResponseDto
                {
                    AlbumId = t.AlbumId,
                    AudioUrl = t.AudioUrl,
                    CoverImgUrl = t.CoverImgUrl,
                    DurationInSeconds = t.DurationInSeconds,
                    Id = t.Id,
                    SubtitleContent = SubtitleEncodeHelper.EncodeSubtitle(t.SubtitleContent),
                    Title = t.Title
                }).FirstOrDefaultAsync();
                return episode;
            });

         
            return Ok(ApiResponse<EpisodeResponseDto?>.Ok(info));
        }

        [HttpGet("ListByAlbum/{albumId}")]
        public async Task<ActionResult<ApiResponse<List<EpisodeResponseDto>>>> FindByAlbumId(long albumId)
        {
            var info = await memoryCacheHelper.GetOrCreateAsync<List<EpisodeResponseDto>>($"EpisodeController_FindByAlbumId_"+ albumId, async entry =>
            {
                // var episodes = await repository.GetAllByAlumIdAsync(albumId);
                // return mapper.ToDtos(episodes.Where(t=>t.IsShow==true).ToList());

                var episodes = await repository.Query().Where(t => t.AlbumId == albumId&&t.IsShow==true).Select(t => new EpisodeResponseDto
                {
                    AlbumId = t.AlbumId,
                    AudioUrl = t.AudioUrl,
                    CoverImgUrl = t.CoverImgUrl,
                    DurationInSeconds = t.DurationInSeconds,
                    Id = t.Id,
                    SubtitleContent = "",
                    Title = t.Title
                }).ToListAsync();
                return episodes;
            });
            return Ok(ApiResponse<List<EpisodeResponseDto>>.Ok(info));
        }

    }
}
