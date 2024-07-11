using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels.Account;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Account;
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
        [HttpGet]
        [Route("info")]
        public async Task<ActionResult<GetAccountWithOderReponse>> GetAll()
        {
            return await _serviceFactory.AccountService.GetAllAccountWithOrderDetailsCustomer();
        }
        [HttpGet]
        [Route("view-my-info")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<ActionResult<CustomerResponseModel>> ViewCustomerDetail()
        {
            return await _serviceFactory.AccountService.ViewMyInfo(HttpContext.User);
        }
    }
}