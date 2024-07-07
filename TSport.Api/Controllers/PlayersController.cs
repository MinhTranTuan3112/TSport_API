using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
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
            return await _serviceFactory.PlayerService.GetPagedPlayers(request);
        }

        [HttpGet]
        [Route("getall")]

        public async Task<ActionResult<List<ViewReponse>>> GetAllPlayers()
        {
            return await _serviceFactory.PlayerService.GetAllPlayers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPlayerDetailsModel>> GetPlayerDetailsById(int id)
        {
            return await _serviceFactory.PlayerService.GetPlayerDetailsById(id);
        }

        [HttpPost]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult<GetPlayerModel>> CreatePlayer([FromBody] CreatePlayerRequest request)
        {
            return Created(nameof(CreatePlayer), await _serviceFactory.PlayerService.CreatePlayer(request, User));
        }

        [HttpPut("{id}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> UpdatePlayer([FromRoute] int id, [FromBody] UpdatePlayerRequest request)
        {
            await _serviceFactory.PlayerService.UpdatePlayer(id, request, User);
            return NoContent();
        }

    }
}