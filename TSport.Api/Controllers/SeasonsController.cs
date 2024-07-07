using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Services.BusinessModels.Season;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeasonsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public SeasonsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<GetSeasonModel>>> GetPagedSeasons([FromQuery] QueryPagedSeasonRequest request)
        {
            return await _serviceFactory.SeasonService.GetPagedSeasons(request);
        }
        [HttpGet]
        [Route("getall")]

        public async Task<ActionResult<List<ViewReponse>>> GetAllSeasons()
        {
            return await _serviceFactory.SeasonService.GetAllSeasons();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetSeasonDetailsModel>> GetSeasonDetailsById(int id)
        {
            return await _serviceFactory.SeasonService.GetSeasonDetailsById(id);
        }

        [HttpPost]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult<GetSeasonModel>> CreateSeason([FromBody] CreateSeasonRequest request)
        {
            return Created(nameof(CreateSeason), await _serviceFactory.SeasonService.CreateSeason(request, HttpContext.User));
        }

        [HttpPut("{id}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> UpdateSeason([FromRoute] int id, [FromBody] UpdateSeasonRequest request)
        {
            await _serviceFactory.SeasonService.UpdateSeason(id, request, HttpContext.User);
            return NoContent();
        }

        [HttpDelete("{seasonId}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<IActionResult> DeleteSeason(int seasonId)
        {
            var result = await _serviceFactory.SeasonService.DeleteSeasonAsync(seasonId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}