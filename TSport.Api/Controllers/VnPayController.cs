using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TSport.Api.Attributes;
using TSport.Api.Models.Payment;
using TSport.Api.Services.Interfaces;
using TSport.Api.Services.Services;

namespace TSport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnPayController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public VnPayController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        [HttpPost("create-payment-url")]
        [SupabaseAuthorize(Roles = ["Customer"])]

        public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentInformationModel model)
        {
            var paymentUrl = _serviceFactory.VnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(Task.FromResult(paymentUrl));
        }

        [HttpGet("payment-callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            // Extract claims from the current user
            var claims = HttpContext.User.Claims;
            var accountIdClaim = claims.FirstOrDefault(c => c.Type == "AccountId");
            int accountId = 0;

                // Extract claims from the current user (you might use User.Claims or HttpContext.User.Claims)
                var claims = HttpContext.User;

                // Call AddtoPayment with the extracted claims
                    await _serviceFactory.PaymentService.AddtoPayment(paymentResponseModel);}
          /*  }
            else
            {
                int.TryParse(accountIdClaim.Value, out accountId);
            }

            var response = _serviceFactory.VnPayService.PaymentExecute(Request.Query, accountId);

            return Ok(new JsonResponse<PaymentResponseModel>(response));
        }
    }
}
