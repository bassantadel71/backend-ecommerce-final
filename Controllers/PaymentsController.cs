using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Stripe;

namespace PaymentApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PaymentsController : ControllerBase
  {
    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymnent ([FromBody] PaymentRequest request)
    {

      var service = new PaymentIntentService();

      var intent = await service.CreateAsync(new PaymentIntentCreateOptions
      {
        Amount = request.Amount,
        Currency = "usd",
        //AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
        //{
        //  Enabled = true
        //}
        PaymentMethodTypes = new List<string>
    {
        "card"
    }
      });

      return Ok(new
      {
        clientSecret = intent.ClientSecret
      });
    }
  }
}

public class PaymentRequest
{
  public long Amount { get; set; }
}
