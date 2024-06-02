using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<GetShirtDetailResponse>> ViewShirtDetail(int id)
        {
            return await _serviceFactory.ShirtService.GetShirtDetailById(id);
        }
    }
}
