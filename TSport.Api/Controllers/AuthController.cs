using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Models.ResponseModels.Auth;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public AuthController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthTokensResponse>> Login([FromBody] LoginRequest request)
        {
            return Created(nameof(Login), await _serviceFactory.AuthService.Login(request));
        }


        [HttpPost("register")]
        public async Task<ActionResult> RegisterAccount([FromBody] RegisterRequest request)
        {
            await _serviceFactory.AuthService.RegisterAccount(request);
            return Ok();
        }

        [HttpGet("who-am-i")]
        [Authorize]

        public async Task<ActionResult<GetAccountResponse>> WhoAmI()
        {
            return await _serviceFactory.AuthService.GetAuthAccountInfoFromSupabaseClaims(HttpContext.User);
        }
    }
}