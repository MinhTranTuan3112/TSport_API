using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels.Club;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Club;
using TSport.Api.Services.BusinessModels.Club;
using TSport.Api.Services.Interfaces;
using TSport.Api.Services.Services;

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
        [Route("getall")]

        public async Task<ActionResult<List<ViewReponse>>> GetAllClubs()
        {
            return await _serviceFactory.ClubService.GetAllClubs();
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

        [HttpDelete("{id}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> DeleteClub(int id)
        {
            await _serviceFactory.ClubService.DeleteClub(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> UpdateClub(int id, [FromQuery] UpdateClubRequest updateClub)
        {
            await _serviceFactory.ClubService.UpdateClub(id, updateClub, HttpContext.User);
            return NoContent();
        }

        [HttpGet("revenue/{clubId}")]
        public async Task<IActionResult> GetRevenueBasedOnClub(int clubId)
        {
            if (clubId <= 0)
            {
                return BadRequest("Invalid club ID.");
            }

            try
            {
                var revenue = await _serviceFactory.OrderService.GetRevenueBasedOnClub(clubId);
                return Ok(new { ClubId = clubId, Revenue = revenue });
            }
            catch (Exception ex)
            {
                // You may want to log the exception or handle it accordingly
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("TotalOrder/{clubId}")]
        public async Task<IActionResult> GetTotalBasedOnClub(int clubId)
        {
            if (clubId <= 0)
            {
                return BadRequest("Invalid club ID.");
            }

            try
            {
                var totalOrder = await _serviceFactory.OrderService.CountOrdersByClubIdAsync(clubId);
                return Ok(new { ClubId = clubId, TotalOrder = totalOrder });
            }
            catch (Exception ex)
            {
                // You may want to log the exception or handle it accordingly
                return StatusCode(500, new { Error = ex.Message });
            }
        }

    }
}
