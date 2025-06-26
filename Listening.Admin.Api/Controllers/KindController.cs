using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Infrastructure.SharedKernel;
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

        [HttpPost]
        [PermissionKey("Kind.Add")]
        public async Task<ActionResult<ApiResponse<long>>> Add(AddKindRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);
            //just several fields are required, so I do not use mapper here
            var info = await domainService.AddAsync(dto.Title, dto.CoverImgUrl);


            return Ok(ApiResponse<long>.Ok(info.Id));
        }

        [HttpPut("{id}")]
        [PermissionKey("Kind.Update")]
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
        [PermissionKey("Kind.Delete")]
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
        [PermissionKey("Kind.Hide")]
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
        [PermissionKey("Kind.Show")]
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

        [HttpPut("Sort")]
        [PermissionKey("Kind.Sort")]
        public async Task<ActionResult<ApiResponse<string>>> Sort(SortRequestDto req)
        {
            await domainService.SortAsync(req.Ids);

            return Ok(ApiResponse<string>.Ok("success"));
        }
    }
}
