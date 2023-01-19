using Amazon.API.Errors;
using Amazon.Core.Entities;
using Amazon.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System;
using System.Threading.Tasks;
using Amazon.Core.Entities.Order_Aggregate;
using Microsoft.Extensions.Logging;

namespace Amazon.API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        private const string _whSecret = "whsec_90dec47dab39d2ee465856ffce3abb1b74b94be6635beec469c8d9365c854943";

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        // Post : /api/Payment/basket1
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null)
                return BadRequest(new ApiResponse(400, " A Problem With Your Basket "));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> Stripewebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _whSecret);

            PaymentIntent intent;
            Order order;

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    order = await _paymentService.UpdatePaymentIntentSucceededOrFailed(intent.Id, true);
                    _logger.LogInformation("Payment Succeeded :)");
                    break;
                case Events.PaymentIntentPaymentFailed:
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    order = await _paymentService.UpdatePaymentIntentSucceededOrFailed(intent.Id, false);
                    _logger.LogInformation("Payment Failed :(");
                    break;
            }
            return new EmptyResult();
        }
    }
}
