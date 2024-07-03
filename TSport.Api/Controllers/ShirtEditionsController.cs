using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.RequestModels.ShirtEdition;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Season;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtEditionsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public ShirtEditionsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
      

        [HttpGet("{id}")]
        public async Task<ActionResult<ShirtEdition>> GetShirtEditionDetailsById(int id)
        {
            return await _serviceFactory.ShirtEditionService.GetShirteditionDetailsById(id);
        }

        [HttpPost]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult<ShirtEdition>> CreateShirtEdition([FromBody] ShirtEditionRequest request)
        {
            return Created(nameof(CreateShirtEdition), await _serviceFactory.ShirtEditionService.CreateShirtEdition(request, HttpContext.User));
        }

        [HttpPut("{id}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> UpdateSeason([FromRoute] int id, [FromBody] ShirtEditionRequest request)
        {
            await _serviceFactory.ShirtEditionService.UpdateShirtEdition(id, request, HttpContext.User);
            return NoContent();
        }

        [HttpDelete("{seasonId}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<IActionResult> DeleteSeason(int seasonId)
        {
            var result = await _serviceFactory.ShirtEditionService.DeleteShirtEditionAsync(seasonId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
