using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Listening.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(
           ICategoryRepository repository) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Category?>>> FindById(long id)
        {
            var info = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return Ok(ApiResponse<Category?>.Ok(info));
        }

        [HttpGet("ListByKind/{kindId}")]
        public async Task<ActionResult<ApiResponse<List<Category>>>> FindByKindId(long kindId)
        {
            var info = await repository.GetAllByKindIdAsync(kindId);

            return Ok(ApiResponse<List<Category>>.Ok(info));
        }

    }
}

