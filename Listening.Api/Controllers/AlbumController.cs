using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Listening.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Listening.Api.Controllers
{
    //  [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController(
            IAlbumRepository repository) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Album?>>> FindById(long id)
        {
            var album = await repository.GetByIdAsync(id);
            //just for admin use,return All album info
            return Ok(ApiResponse<Album?>.Ok(album));
        }

        [HttpGet("ListByCatagory/{categoryId}")]
        public async Task<ActionResult<ApiResponse<List<Album>>>> FindByCategoryId(long categoryId)
        {
            var albums = await repository.GetAllByCategoryIdAsync(categoryId);

            return Ok(ApiResponse<List<Album>>.Ok(albums));
        }

    
    }
}
