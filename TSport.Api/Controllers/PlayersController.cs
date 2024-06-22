using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Models.RequestModels.Player;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Services.BusinessModels.Player;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public PlayersController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultResponse<GetPlayerModel>>> GetPagedPlayers([FromQuery] QueryPagedPlayersRequest request)
        {
            return await _serviceFactory.PlayerService.GetCachedPagedPlayers(request);
        }


    }
}