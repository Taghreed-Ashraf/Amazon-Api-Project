using Amazon.Core.Entities.Order_Aggregate;
using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using Amazon.Core.Services;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Amazon.Core.Entities.Product;
using Amazon.Core.Specification;

namespace Amazon.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];

            var basket = await _basketRepository.GetBasketByIdAsync(basketId);
            if (basket == null) return null;

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Reposoitory<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.shippingPrice = shippingPrice;
            }
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Reposoitory<Product>().GetByIdAsync(item.Id);
                if (item.Price != product.Price)
                    item.Price = product.Price;
            }
            var service = new PaymentIntentService();

            PaymentIntent intent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }

                };


                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketRepository.UpdateBasketAsync(basket);

            return basket;
        }


        public async Task<Order> UpdatePaymentIntentSucceededOrFailed(string paymentIntentId, bool Issuccess)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Reposoitory<Order>().GetByIdWithSpecAsync(spec);

            if (Issuccess)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            _unitOfWork.Reposoitory<Order>().Update(order);

            await _unitOfWork.Complete();

            return order;
        }
    }
}
