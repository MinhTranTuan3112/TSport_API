using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.BusinessLogic.Interfaces;
using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Club;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Club;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClubsController : ControllerBase
 {
        private readonly IServiceFactory _serviceFactory;

        public ClubsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<GetClubModel>>> GetClub([FromQuery] QueryCLubRequest query)
        {
            return await _serviceFactory.ClubService.GetPagedClub(query);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GetClubModel>> GetShirtDetailsById(int id)
        {
            return await _serviceFactory.ClubService.GetClubByClubId(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateClubResponse>> CreateClub([FromBody] CreateClubRequest createShirtRequest)
        {
            var result = await _serviceFactory.ClubService.AddClub(createShirtRequest, HttpContext.User);
            return Created(nameof(CreateClub), result);
        }
    }
}
