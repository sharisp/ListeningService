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
    public class EpisodeController(
           IEpisodeRepository repository) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Episode?>>> FindById(long id)
        {
            var info = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return Ok(ApiResponse<Episode?>.Ok(info));
        }

        [HttpGet("ListByAlbum/{albumId}")]
        public async Task<ActionResult<ApiResponse<List<Episode>>>> FindByKindId(long albumId)
        {
            var info = await repository.GetAllByAlumIdAsync(albumId);

            return Ok(ApiResponse<List<Episode>>.Ok(info));
        }

    }
}
