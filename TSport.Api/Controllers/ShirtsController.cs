using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSport.Api.Attributes;
using TSport.Api.Models.RequestModels.Shirt;
using TSport.Api.Models.ResponseModels;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Repositories.Entities;
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
        private readonly ILogger<ShirtsController> _logger;

        public ShirtsController(IServiceFactory serviceFactory, ILogger<ShirtsController> logger)
        {
            _serviceFactory = serviceFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<PagedResultResponse<GetShirtModel>> GetPagedShirts([FromQuery] QueryPagedShirtsRequest request)
        {
            _logger.LogInformation(HttpContext.User.ToString());
            return await _serviceFactory.ShirtService.GetPagedShirts(request);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShirtDetailModel>> GetShirtDetailsById(int id)
        {
            return await _serviceFactory.ShirtService.GetShirtDetailById(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateShirtResponse>> CreateShirt([FromForm] CreateShirtRequest createShirtRequest)
        {
            var result = await _serviceFactory.ShirtService.AddShirt(createShirtRequest, HttpContext.User);
            return Created(nameof(CreateShirt), result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteShirt(int id)
        {
            await _serviceFactory.ShirtService.DeleteShirt(id);
            return Ok();
        }
    }
}