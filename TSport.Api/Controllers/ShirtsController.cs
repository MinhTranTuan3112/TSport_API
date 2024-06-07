using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Services.BusinessModels;
using TSport.Api.Services.BusinessModels.Shirt;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public ShirtsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public async Task<PagedResultResponse<GetShirtModel>> GetPagedShirts([FromQuery] QueryPagedShirtsRequest request)
        {
            return await _serviceFactory.ShirtService.GetPagedShirts(request);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShirtDetailModel>> GetShirtDetailsById(int id)
        {
            return await _serviceFactory.ShirtService.GetShirtDetailById(id);
        }
    }
}