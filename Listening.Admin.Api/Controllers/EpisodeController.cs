﻿using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Identity.Api;
using Listening.Admin.Api.Attributes;
using Listening.Admin.Api.Dtos.Request;
using Listening.Admin.Api.Dtos.Response;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Listening.Domain.Services;
using Listening.Infrastructure.Extensions;
using Listening.Infrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Listening.Admin.Api.Controllers
{

    [Authorize]
    // [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeController(
          IEpisodeRepository repository,
          EpisodeDomainService domainService, ICurrentUser currentUser, IValidator<AddEpisodeRequestDto> validator, IValidator<UpdateRequestDto> updateValidator, IValidator<UpdateEpisodeRequestDto> editValidator) : ControllerBase
    {

        [HttpGet("{id}")]
        [PermissionKey("Episode.FindById")]
        public async Task<ActionResult<ApiResponse<EpisodeResponseDto?>>> FindById(long id)
        {
            var query = repository.Query().Where(t => t.Id == id);

            var res = await query.Select(t => EpisodeResponseDto.ToDto(t, false)).FirstOrDefaultAsync();
            return Ok(ApiResponse<EpisodeResponseDto?>.Ok(res));
        }

        [HttpGet("ListByAlbum/{albumId}")]
        [PermissionKey("Episode.List")]
        public async Task<ActionResult<ApiResponse<List<EpisodeResponseDto>>>> FindByAlbumId(long albumId)
        {
            var query = repository.Query().Where(t => t.AlbumId == albumId);


            var res = await query.Select(t => EpisodeResponseDto.ToDto(t, true)).ToListAsync();
            return Ok(ApiResponse<List<EpisodeResponseDto>>.Ok(res));
        }
        [HttpGet("Pagination")]
        [PermissionKey("Episode.List")]
        public async Task<ActionResult<ApiResponse<PaginationResponse<EpisodeResponseDto>>>> Pagination(long albumId = 0, string title = "", int pageIndex = 1, int pageSize = 10)
        {
            var query = repository.Query();

            if (albumId > 0)
            {
                query = query.Where(t => t.AlbumId == albumId);
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t => t.Title.Contains(title.Trim()));
            }

            var res = await query.Select(t => EpisodeResponseDto.ToDto(t, true)).ToPaginationResponseAsync(pageIndex, pageSize);

            return this.OkResponse(res);
        }
        [HttpPost]
        [PermissionKey("Episode.Add")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Add(AddEpisodeRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var info = await domainService.AddAsync(dto.AlbumId, dto.Title, dto.SubtitleType, dto.SubtitleContent, dto.AudioUrl, dto.DurationInSeconds, dto.CoverImgUrl);

            return Ok(ApiResponse<long>.Ok(info.Id));
        }

        [HttpPut("{id}")]
        [PermissionKey("Episode.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, UpdateEpisodeRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, editValidator);
            var info = await repository.GetByIdAsync(id);
            if (info == null)
            {
                return this.FailResponse("not exist");
            }
            info.ChangeTitle(dto.Title);
            info.ChangeSequenceNumber(dto.SequenceNumber);

            if (dto.CoverImgUrl != null)
            {
                info.ChangeCoverImgUrl(dto.CoverImgUrl);
            }
            info.ChangeAudioUrl(dto.AudioUrl);
            info.ChangeSubtitleContent(dto.SubtitleType, dto.SubtitleContent, 0);


            return this.OkResponse(id);
        }
        [HttpPut("ChangeTitle/{id}")]
        [PermissionKey("Episode.ChangeTitle")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> ChangeTitle(long id, UpdateRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, updateValidator);
            var info = await repository.GetByIdAsync(id);
            if (info == null)
            {
                return this.FailResponse("not exist");
            }
            info.ChangeTitle(dto.Title);

            return this.OkResponse(id);
        }

        [HttpDelete("{id}")]
        [PermissionKey("Episode.Delete")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Delete(long id)
        {
            var album = await repository.GetByIdAsync(id);
            if (album == null)
            {
                return this.FailResponse("not exist");
            }
            album.SoftDelete(currentUser);

            return this.OkResponse(id);
        }

        [HttpPut("Hide/{id}")]
        [PermissionKey("Episode.Hide")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Hide(long id)
        {
            var album = await repository.GetByIdAsync(id);
            if (album == null)
            {
                return this.FailResponse("not exist");
            }
            album.Hide();

            return this.OkResponse(id);
        }

        [HttpPut("Show/{id}")]
        [PermissionKey("Episode.Show")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Show(long id)
        {
            var album = await repository.GetByIdAsync(id);
            if (album == null)
            {
                return this.FailResponse("not exist");
            }
            album.Show();

            return this.OkResponse(id);
        }

        [HttpPut("Sort/{albumId}")]
        [PermissionKey("Episode.Sort")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Sort(long albumId, SortRequestDto req)
        {
            await domainService.SortAsync(albumId, req.Ids);

            return this.OkResponse(albumId);
        }
    }
}
