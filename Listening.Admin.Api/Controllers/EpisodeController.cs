using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Listening.Admin.Api.Attributes;
using Listening.Admin.Api.Dtos.Request;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Listening.Domain.Services;
using Listening.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Admin.Api.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeController(
           IEpisodeRepository repository,
           EpisodeDomainService domainService, ICurrentUser currentUser, IValidator<AddEpisodeRequestDto> validator, IValidator<UpdateRequestDto> updateValidator) : ControllerBase
    {

        [HttpGet("{id}")]
        [PermissionKey("Episode.FindById")]
        public async Task<ActionResult<ApiResponse<Episode?>>> FindById(long id)
        {
            var info = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return Ok(ApiResponse<Episode?>.Ok(info));
        }

        [HttpGet("ListByAlbum/{albumId}")]
        [PermissionKey("Episode.FindByKindId")]
        public async Task<ActionResult<ApiResponse<List<Episode>>>> FindByKindId(long albumId)
        {
            var info = await repository.GetAllByAlumIdAsync(albumId);

            return Ok(ApiResponse<List<Episode>>.Ok(info));
        }

        [HttpPost]
        [PermissionKey("Episode.Add")]
        public async Task<ActionResult<ApiResponse<long>>> Add(AddEpisodeRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);
            //just several fields are required, so I do not use mapper here
            var info = await domainService.AddAsync(dto.AlumId, dto.Title, dto.SubtitleType, dto.SubtitleContent, dto.AudioUrl, dto.DurationInSeconds, dto.CoverImgUrl);
            //  long alumId, string title, string subtitleType, string subtitleContent, Uri audioUrl, long durationInSeconds, Uri? coverImgUrl
            return Ok(ApiResponse<long>.Ok(info.Id));
        }

        [HttpPut("ChangeTitle/{id}")]
        [PermissionKey("Episode.ChangeTitle")]
        public async Task<ActionResult<ApiResponse<string>>> ChangeTitle(long id, UpdateRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, updateValidator);
            var album = await repository.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound(ApiResponse<string>.Fail("not exists"));
            }
            album.ChangeTitle(dto.Title);

            return Ok(ApiResponse<string>.Ok("success"));
        }

        [HttpDelete("{id}")]
        [PermissionKey("Episode.Delete")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(long id)
        {
            var album = await repository.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound(ApiResponse<string>.Fail("not exists"));
            }
            album.SoftDelete(currentUser);

            return Ok(ApiResponse<string>.Ok("success"));
        }

        [HttpPut("Hide/{id}")]
        [PermissionKey("Episode.Hide")]
        public async Task<ActionResult<ApiResponse<string>>> Hide(long id)
        {
            var album = await repository.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound(ApiResponse<string>.Fail("not exists"));
            }
            album.Hide();

            return Ok(ApiResponse<string>.Ok("success"));
        }

        [HttpPut("Show/{id}")]
        [PermissionKey("Episode.Show")]
        public async Task<ActionResult<ApiResponse<string>>> Show(long id)
        {
            var album = await repository.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound(ApiResponse<string>.Fail("not exists"));
            }
            album.Show();

            return Ok(ApiResponse<string>.Ok("success"));
        }

        [HttpPut("Sort/{albumId}")]
        [PermissionKey("Episode.Sort")]
        public async Task<ActionResult<ApiResponse<string>>> Sort(long albumId, SortRequestDto req)
        {
            await domainService.SortAsync(albumId, req.Ids);

            return Ok(ApiResponse<string>.Ok("success"));
        }
    }
}
