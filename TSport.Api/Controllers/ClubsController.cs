using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
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
        public async Task<ActionResult<PagedResultResponse<GetClubModel>>> GetPagedClubs([FromQuery] QueryClubRequest query)
        {
            return await _serviceFactory.ClubService.GetPagedClubs(query);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetClubDetailsModel>> GetClubDetailsById(int id)
        {
            return await _serviceFactory.ClubService.GetClubDetailsById(id);
        }

        [HttpPost]
        [SupabaseAuthorize(Roles = ["Staff"])]

        public async Task<ActionResult<GetClubResponse>> CreateClub(CreateClubRequest createClubRequest)
        {
            var result = await _serviceFactory.ClubService.AddClub(createClubRequest, HttpContext.User);
            return Created(nameof(CreateClub), result);
        }

        [HttpDelete]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> DeleteClub(int id)
        {
            await _serviceFactory.ClubService.DeleteClub(id);
            return Ok();
        }

        [HttpPut]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> UpdateClub([FromQuery] UpdateClubRequest updateClub)
        {
            await _serviceFactory.ClubService.UpdateClub(updateClub, HttpContext.User);
            return NoContent();
        }

    }
}
