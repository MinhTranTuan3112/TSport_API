using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels.Season;
using TSport.Api.Models.RequestModels.ShirtEdition;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Repositories.Entities;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Season;
using TSport.Api.Services.BusinessModels.ShirtEdition;
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

        [HttpGet("getall")]
        public async Task<ActionResult<List<GetShirtEdtionModel>>> GetShirtEditions()
        {
            return await _serviceFactory.ShirtEditionService.GetShirtEditions();
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<GetShirtEdtionModel>>> GetPagedShirtEditions([FromQuery] QueryPagedShirtEditionRequest request)
        {
            return await _serviceFactory.ShirtEditionService.GetPagedShirtEditions(request);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ShirtEditionDetailsModel>> GetShirtEditionDetailsById(int id)
        {
            return await _serviceFactory.ShirtEditionService.GetShirtEditionDetailsById(id);
        }

        [HttpPost]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult<ShirtEditionModel>> CreateShirtEdition([FromBody] CreateShirtEditionRequest request)
        {
            return Created(nameof(CreateShirtEdition), await _serviceFactory.ShirtEditionService.CreateShirtEdition(request, HttpContext.User));
        }

        [HttpPut("{id}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> UpdateShirtEdition([FromRoute] int id, [FromBody] UpdateShirtEditionRequest request)
        {
            await _serviceFactory.ShirtEditionService.UpdateShirtEdition(id, request, HttpContext.User);
            return NoContent();
        }

        [HttpDelete("{seasonId}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<IActionResult> DeleteShirtEdition(int shirtEditionId)
        {
            await _serviceFactory.ShirtEditionService.DeleteShirtEdition(shirtEditionId);

            return NoContent();
        }
    }
}
