using Microsoft.AspNetCore.Mvc;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.ResponseModels.Shirt;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public ShirtController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [HttpPost]
        public async Task<CreateShirtResponse> CreateShirt([FromBody] CreateShirtRequest createShirtRequest)
        {
            var result = await _serviceFactory.ShirtService.AddShirt(createShirtRequest, HttpContext.User);
            return result;
        }
    }
}
