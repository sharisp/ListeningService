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
    public class CategoryController(
           ICategoryRepository repository,
           CategoryDomainService domainService, ICurrentUser currentUser, IValidator<CategoryRequestDto> validator, IValidator<UpdateRequestDto> updateValidator) : ControllerBase
    {

        [HttpGet("{id}")]
        [PermissionKey("Category.List")]
        public async Task<ActionResult<ApiResponse<Category?>>> FindById(long id)
        {
            var info = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return Ok(ApiResponse<Category?>.Ok(info));
        }
        [HttpGet("Pagination")]
        [PermissionKey("Category.List")]
        public async Task<ActionResult<ApiResponse<PaginationResponse<Category>>>> Pagination(long kindId=0, string title = "", int pageIndex = 1, int pageSize = 10)
        {
            var query = repository.Query();

            if (kindId > 0)
            {
                query = query.Where(t => t.KindId == kindId);
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t => t.Title.Contains(title.Trim()));
            }
            var res = await query.ToPaginationResponseAsync(pageIndex, pageSize);

            return this.OkResponse(res);
        }
        [HttpGet("ListByKind/{kindId}")]
        [PermissionKey("Category.List")]
        public async Task<ActionResult<ApiResponse<List<Category>>>> FindByKindId(long kindId)
        {
            var info = await repository.GetAllByKindIdAsync(kindId);

            return Ok(ApiResponse<List<Category>>.Ok(info));
        }

        [HttpPost]
        [PermissionKey("Category.Add")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Add(CategoryRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);
            //just several fields are required, so I do not use mapper here
            var info = await domainService.AddAsync(dto.Title, dto.KindId, dto.CoverImgUrl);

            return Ok(ApiResponse<long>.Ok(info.Id));
        }

        [HttpPut("{id}")]
        [PermissionKey("Category.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, UpdateRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, updateValidator);
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


            return this.OkResponse(id);
        }

        [HttpDelete("{id}")]
        [PermissionKey("Category.Delete")]
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
        [PermissionKey("Category.Hide")]
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
        [PermissionKey("Category.Show")]
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

        [HttpPut("Sort/{kindId}")]
        [PermissionKey("Category.Sort")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Sort(long kindId, SortRequestDto req)
        {
            await domainService.SortAsync(kindId, req.Ids);

             return this.OkResponse(kindId);
        }
    }
}

