using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels.Account;
using TSport.Api.Models.RequestModels.Order;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Services.BusinessModels.Account;
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
        public async Task<ActionResult<GetAccountWithOrderReponse>> GetAll()
        {
            return await _serviceFactory.AccountService.GetAllAccountWithOrderDetailsCustomer();
        }


        [HttpGet("customer/details-info")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<ActionResult<CustomerAccountWithOrderInfoModel>> GetCustomerDetailsInfo()
        {
            return await _serviceFactory.AccountService.GetDetailsCutomerInfo(HttpContext.User);
        }
        
        [HttpGet]
        [Route("customer/basic-info")]
        [SupabaseAuthorize(Roles = ["Customer"])]
        public async Task<ActionResult<GetAccountResponse>> GetCustomerBasicInfo()
        {
            return await _serviceFactory.AccountService.GetCustomerInfo(HttpContext.User);
        }


    }
}