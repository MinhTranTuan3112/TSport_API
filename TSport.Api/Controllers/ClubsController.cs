using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Club;
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
        
        public async Task<ActionResult<CreateClubResponse>> CreateClub( CreateClubRequest createShirtRequest)
        {
            var result = await _serviceFactory.ClubService.AddClub(createShirtRequest, HttpContext.User);
            return Created(nameof(CreateClub), result);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteClub(int id)
        {
            await _serviceFactory.ClubService.DeleteClub(id);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<CreateClubResponse>> UpdateClub([FromQuery] UpdateClubRequest updateClub)
        {
            var result = await _serviceFactory.ClubService.UpdateClub(updateClub, HttpContext.User);
            return Created(nameof(UpdateClub), result);
        }
    }
}
