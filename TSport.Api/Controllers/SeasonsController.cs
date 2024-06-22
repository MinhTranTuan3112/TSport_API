using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}