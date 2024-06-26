using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Models.RequestModels.Account;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public AccountsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpPut("customers")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> UpdateCustomerAccountInfo([FromBody] UpdateCustomerInfoRequest request)
        {
            await _serviceFactory.AccountService.UpdateCustomerInfo(HttpContext.User, request);   
            return NoContent();
        }
    }
}