using Microsoft.AspNetCore.Mvc;
using TSport.Api.Models.Entities;
using TSport.Api.Models.RequestModels;
using TSport.Api.Models.ResponseModels.Auth;
using TSport.Api.Models.ResponseModels.Cart;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public CartController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }


        [HttpGet("getCart")]
        public async Task<ActionResult<CartResponse>> getCartbyId(int userid)
        {
                return Ok( await _serviceFactory.OrderService.GetCartInfo(userid));
        }

        [HttpPost("add-to-cart")]
        public async  Task<ActionResult> AddtoCart([FromBody] AddToCartRequest request )
        {
            await _serviceFactory.OrderDetailsService.AddToCart(request.UserId, request.ShirtId,request.Quantity.Value );
            return Ok();
        }

        /*  public async Task<ActionResult<AuthTokensResponse>> Login([FromBody] LoginRequest request)
          {
              return Created(nameof(Login), await _serviceFactory.AuthService.Login(request));
          }*/
        /*
                public IActionResult Index()
                {
                    return View();
                }*/
    }
}
