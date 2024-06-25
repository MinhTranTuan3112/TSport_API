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

        public ShirtsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public async Task<PagedResultResponse<GetShirtInPagingResultModel>> GetPagedShirts([FromQuery] QueryPagedShirtsRequest request)
        {
            return await _serviceFactory.ShirtService.GetPagedShirts(request);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShirtDetailModel>> GetShirtDetailsById(int id)
        {
            return await _serviceFactory.ShirtService.GetShirtDetailById(id);
        }

        [HttpPost]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult<CreateShirtResponse>> CreateShirt([FromForm] CreateShirtRequest createShirtRequest)
        {
            var result = await _serviceFactory.ShirtService.AddShirt(createShirtRequest, HttpContext.User);
            return Created(nameof(CreateShirt), result);
        }

        [HttpDelete("{id}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> DeleteShirt(int id)
        {
            await _serviceFactory.ShirtService.DeleteShirt(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [SupabaseAuthorize(Roles = ["Staff"])]
        public async Task<ActionResult> UpdateShirt([FromRoute] int id, [FromForm] UpdateShirtRequest request)
        {
            await _serviceFactory.ShirtService.UpdateShirt(id, request, HttpContext.User);
            return NoContent();
        }
    }
}