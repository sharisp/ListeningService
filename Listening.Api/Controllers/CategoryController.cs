using Domain.SharedKernel.Interfaces;
using FluentValidation;
using IdGen;
using Listening.Api.Dtos;
using Listening.Api.Dtos.Mapper;
using Listening.Api.Helpers;
using Listening.Domain.Entities;
using Listening.Domain.Interfaces;
using Listening.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Listening.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(
           ICategoryRepository repository, MemoryCacheHelper memoryCacheHelper, BaseEntityMapper mapper) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BaseEntityResponseDto?>>> FindById(long id)
        {

            var dto = await memoryCacheHelper.GetOrCreateAsync<BaseEntityResponseDto?>($"CategoryController_FindById_"+ id, async entry =>
            {
                var info = await repository.GetByIdAsync(id);
                return mapper.ToDto(info);
            });
            //just for admin use,return All album info
            return Ok(ApiResponse<BaseEntityResponseDto?>.Ok(dto));
        }

        [HttpGet("List")]
        public async Task<ActionResult<ApiResponse<List<BaseEntityResponseDto>>>> List()
        {
            var responseDtos = await memoryCacheHelper.GetOrCreateAsync<List<BaseEntityResponseDto>>($"CategoryController_List" , async entry =>
            {
                var categories = await repository.Query().Where(t=>t.IsShow==true).OrderBy(t=>t.SequenceNumber).ToListAsync();
                var dtos = new List<BaseEntityResponseDto>();
                foreach (var item in categories.Where(t => t.IsShow == true))
                {
                    dtos.Add(mapper.ToDto(item));
                }
                return dtos;
            });
            return Ok(ApiResponse<List<BaseEntityResponseDto>>.Ok(responseDtos));
        }
        [HttpGet("ListByKind/{kindId}")]
        public async Task<ActionResult<ApiResponse<List<BaseEntityResponseDto>>>> FindByKindId(long kindId)
        {
            var responseDtos = await memoryCacheHelper.GetOrCreateAsync<List<BaseEntityResponseDto>>($"CategoryController_FindByKindId_"+ kindId, async entry =>
            {
                var categories = await repository.GetAllByKindIdAsync(kindId);
                var dtos = new List<BaseEntityResponseDto>();
                foreach (var item in categories.Where(t => t.IsShow == true))
                {
                    dtos.Add(mapper.ToDto(item));
                }
                return dtos;
            });
            return Ok(ApiResponse<List<BaseEntityResponseDto>>.Ok(responseDtos));
        }

    }
}

