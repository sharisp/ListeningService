using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Identity.Api;
using Listening.Admin.Api.Attributes;
using Listening.Admin.Api.Dtos.Request;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Listening.Domain.Services;
using Listening.Infrastructure.Extensions;
using Listening.Infrastructure.Options;
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
        [PermissionKey("Album.List")]
        public async Task<ActionResult<ApiResponse<Album?>>> FindById(long id)
        {
            var album = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return this.OkResponse(album);
        }

        [HttpGet("ListByCatagory/{categoryId}")]
        [PermissionKey("Album.FindByCategoryId")]
        public async Task<ActionResult<ApiResponse<List<Album>>>> FindByCategoryId(long categoryId)
        {
            var albums = await repository.GetAllByCategoryIdAsync(categoryId);

            return this.OkResponse(albums);
        }

        [HttpGet("Pagination")]
        [PermissionKey("Album.List")]
        public async Task<ActionResult<ApiResponse<PaginationResponse<Album>>>> Pagination(long categoryId=0, string title = "", int pageIndex = 1, int pageSize = 10)
        {
            var query = repository.Query();

            if (categoryId > 0)
            {
                query = query.Where(t => t.CategoryId == categoryId);
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t => t.Title.Contains(title.Trim()));
            }
            var res = await query.ToPaginationResponseAsync(pageIndex, pageSize);

            return this.OkResponse(res);
        }
        [HttpPost]
        [PermissionKey("Album.Add")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Add(AddRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);
            //just several fields are required, so I do not use mapper here
            Album album = await domainService.AddAsync(dto.Title, dto.ForeginId, dto.CoverImgUrl);

            return this.OkResponse(album.Id);
        }

        [HttpPut("{id}")]
        [PermissionKey("Album.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, UpdateRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, updateValidator);
            var album = await repository.GetByIdAsync(id);
            if (album == null)
            {
                return this.FailResponse("album not exist");
            }
            album.ChangeTitle(dto.Title);

            return this.OkResponse(id);
        }

        [HttpDelete("{id}")]
        [PermissionKey("Album.Delete")]
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

        [HttpPut]
        [Route("Hide/{id}")]
        [PermissionKey("Album.Hide")]
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
        [PermissionKey("Album.Show")]
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

        [HttpPut("Sort/{categoryId}")]
        [PermissionKey("Album.Sort")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Sort(long categoryId, SortRequestDto req)
        {
            await domainService.SortAsync(categoryId, req.Ids);

            return this.OkResponse(categoryId);
        }
    }
}
