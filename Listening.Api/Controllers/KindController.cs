using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Api.Controllers
{

    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KindController(
           IKindRepository repository) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Kind?>>> FindById(long id)
        {
            var info = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return Ok(ApiResponse<Kind?>.Ok(info));
        }

        [HttpGet("List")]
        public async Task<ActionResult<ApiResponse<List<Kind>>>> GetAll()
        {
            var list = await repository.GetAllAsync();

            return Ok(ApiResponse<List<Kind>>.Ok(list));
        }

    }
}
