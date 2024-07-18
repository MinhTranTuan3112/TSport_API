using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Services.BusinessModels.SeasonPlayer;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeasonPlayersController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public SeasonPlayersController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<SeasonPlayerWithSeasonAndClubModel>>> GetAllSeasonPlayers()
        {
            return await _serviceFactory.SeasonPlayerService.GetSeasonPlayers();
        }
        
    }
}