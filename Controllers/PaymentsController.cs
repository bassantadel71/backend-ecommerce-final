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
    //[HttpPost("create-payment-intent")]
		//public async Task<IActionResult> CreatePaymnent ([FromBody] PaymentRequest request)
		//{

		//  var service = new PaymentIntentService();

		//  var intent = await service.CreateAsync(new PaymentIntentCreateOptions
		//  {
		//    Amount = request.Amount,
		//    Currency = "usd",
		//    //AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
		//    //{
		//    //  Enabled = true
		//    //}
		//    PaymentMethodTypes = new List<string>
		//{
		//    "card"
		//}
		//  });

		//  return Ok(new
		//  {
		//    clientSecret = intent.ClientSecret
		//  });
		//}
		[HttpPost("create-payment-intent")]
		public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
		{
			try
			{
				// Verify Stripe is configured
				if (string.IsNullOrEmpty(StripeConfiguration.ApiKey))
				{
					return StatusCode(500, new { error = "Stripe is not configured properly" });
				}

				// Validate request
				if (request.Amount <= 0)
				{
					return BadRequest(new { error = "Amount must be greater than 0" });
				}

				var service = new PaymentIntentService();

				var intent = await service.CreateAsync(new PaymentIntentCreateOptions
				{
					Amount = request.Amount,
					Currency = "usd",
					PaymentMethodTypes = new List<string> { "card" }
				});

				return Ok(new
				{
					clientSecret = intent.ClientSecret,
					paymentIntentId = intent.Id
				});
			}
			catch (StripeException stripeEx)
			{
				// Log the actual Stripe error
				Console.WriteLine($"Stripe Error: {stripeEx.StripeError?.Message ?? stripeEx.Message}");
				return StatusCode(400, new { error = stripeEx.StripeError?.Message ?? stripeEx.Message });
			}
			catch (Exception ex)
			{
				// Log general errors
				Console.WriteLine($"General Error: {ex.Message}");
				return StatusCode(500, new { error = "An internal error occurred" });
			}
		}
	}
}

public class PaymentRequest
{
  public long Amount { get; set; }
}
