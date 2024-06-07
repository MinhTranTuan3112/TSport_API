using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteShirt(int id)
        {
            await _serviceFactory.ShirtService.DeleteShirt(id);
            return NoContent();
        }
    }
}