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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController(
            IAlbumRepository repository,
            AlbumDomainService domainService, ICurrentUser currentUser, IValidator<AddRequestDto> validator, IValidator<UpdateRequestDto> updateValidator) : ControllerBase
    {

        [HttpGet("{id}")]
        [PermissionKey("Album.FindById")]
        public async Task<ActionResult<ApiResponse<Album?>>> FindById(long id)
        {
            var album = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return Ok(ApiResponse<Album?>.Ok(album));
        }

        [HttpGet("ListByCatagory/{categoryId}")]
        [PermissionKey("Album.FindByCategoryId")]
        public async Task<ActionResult<ApiResponse<List<Album>>>> FindByCategoryId(long categoryId)
        {
            var albums = await repository.GetAllByCategoryIdAsync(categoryId);

            return Ok(ApiResponse<List<Album>>.Ok(albums));
        }

        [HttpPost]
        [PermissionKey("Album.Add")]
        public async Task<ActionResult<ApiResponse<long>>> Add(AddRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);
            //just several fields are required, so I do not use mapper here
            Album album = await domainService.AddAsync(dto.Title, dto.ForeginId, dto.CoverImgUrl);

            return Ok(ApiResponse<long>.Ok(album.Id));
        }

        [HttpPut("{id}")]
        [PermissionKey("Album.Update")]
        public async Task<ActionResult<ApiResponse<string>>> Update(long id, UpdateRequestDto dto)
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
        [PermissionKey("Album.Delete")]
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

        [HttpPut]
        [Route("Hide/{id}")]
        [PermissionKey("Album.Hide")]
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
        [PermissionKey("Album.Show")]
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

        [HttpPut("Sort/{categoryId}")]
        [PermissionKey("Album.Sort")]
        public async Task<ActionResult<ApiResponse<string>>> Sort(long categoryId, SortRequestDto req)
        {
            await domainService.SortAsync(categoryId, req.Ids);

            return Ok(ApiResponse<string>.Ok("success"));
        }
    }
}
