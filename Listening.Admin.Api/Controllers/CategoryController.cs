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
    public class CategoryController(
           ICategoryRepository repository,
           CategoryDomainService domainService, ICurrentUser currentUser, IValidator<AddRequestDto> validator, IValidator<UpdateRequestDto> updateValidator) : ControllerBase
    {

        [HttpGet("{id}")]
        [PermissionKey("Category.FindById")]
        public async Task<ActionResult<ApiResponse<Category?>>> FindById(long id)
        {
            var info = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return Ok(ApiResponse<Category?>.Ok(info));
        }

        [HttpGet("ListByKind/{kindId}")]
        [PermissionKey("Category.FindByKindId")]
        public async Task<ActionResult<ApiResponse<List<Category>>>> FindByKindId(long kindId)
        {
            var info = await repository.GetAllByKindIdAsync(kindId);

            return Ok(ApiResponse<List<Category>>.Ok(info));
        }

        [HttpPost]
        [PermissionKey("Category.Add")]
        public async Task<ActionResult<ApiResponse<long>>> Add(AddRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);
            //just several fields are required, so I do not use mapper here
            var info = await domainService.AddAsync(dto.Title, dto.ForeginId, dto.CoverImgUrl);

            return Ok(ApiResponse<long>.Ok(info.Id));
        }

        [HttpPut("{id}")]
        [PermissionKey("Category.Update")]
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
        [PermissionKey("Category.Delete")]
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
        [PermissionKey("Category.Hide")]
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
        [PermissionKey("Category.Show")]
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

        [HttpPut("Sort/{kindId}")]
        [PermissionKey("Category.Sort")]
        public async Task<ActionResult<ApiResponse<string>>> Sort(long kindId, SortRequestDto req)
        {
            await domainService.SortAsync(kindId, req.Ids);

            return Ok(ApiResponse<string>.Ok("success"));
        }
    }
}

