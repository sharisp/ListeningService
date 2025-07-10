using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Identity.Api;
using Infrastructure.SharedKernel;
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
    public class KindController(
          IKindRepository repository,
          KindDomainService domainService, ICurrentUser currentUser, IValidator<AddKindRequestDto> validator, IValidator<UpdateRequestDto> updateValidator) : ControllerBase
    {

        [HttpGet("{id}")]
        [PermissionKey("Kind.FindById")]
        public async Task<ActionResult<ApiResponse<Kind?>>> FindById(long id)
        {
            var info = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return Ok(ApiResponse<Kind?>.Ok(info));
        }

        [HttpGet("List")]
        [PermissionKey("Kind.GetAll")]
        public async Task<ActionResult<ApiResponse<List<Kind>>>> GetAll()
        {
            var list = await repository.GetAllAsync();

            return Ok(ApiResponse<List<Kind>>.Ok(list));
        }
        [HttpGet("Pagination")]
        [PermissionKey("Kind.List")]
        public async Task<ActionResult<ApiResponse<PaginationResponse<Category>>>> Pagination(string title = "", int pageIndex = 1, int pageSize = 10)
        {
            var query = repository.Query();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t => t.Title.Contains(title.Trim()));
            }

            var res = await query.ToPaginationResponseAsync(pageIndex, pageSize);

            return this.OkResponse(res);
        }

        [HttpPost]
        [PermissionKey("Kind.Add")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Add(AddKindRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);
            //just several fields are required, so I do not use mapper here
            var info = await domainService.AddAsync(dto.Title, dto.CoverImgUrl);


            return Ok(ApiResponse<long>.Ok(info.Id));
        }

        [HttpPut("{id}")]
        [PermissionKey("Kind.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, UpdateRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, updateValidator);
            var album = await repository.GetByIdAsync(id);
            if (album == null)
            {
                return this.FailResponse("not exist");
            }
            album.ChangeTitle(dto.Title);

            return this.OkResponse(id);
        }

        [HttpDelete("{id}")]
        [PermissionKey("Kind.Delete")]
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
        [PermissionKey("Kind.Hide")]
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
        [PermissionKey("Kind.Show")]
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

        [HttpPut("Sort")]
        [PermissionKey("Kind.Sort")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Sort(SortRequestDto req)
        {
            await domainService.SortAsync(req.Ids);

            return this.OkResponse("success");
        }
    }
}
