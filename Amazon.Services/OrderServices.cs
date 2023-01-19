using Amazon.Core.Entities;
using Amazon.Core.Entities.Order_Aggregate;
using Amazon.Core.Repositories;
using Amazon.Core.Services;
using Amazon.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderServices(
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get Basket From Baskkets Repo
            var basket = await _basketRepo.GetBasketByIdAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                //var product = await _productRepo.GetByIdAsync(item.Id);
                var product = await _unitOfWork.Reposoitory<Product>().GetByIdAsync(item.Id);
                var productItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            // 3. Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);


            // 4. Get Delivery Method From DeliveryMethods Repo
            var deliveryMethod = await _unitOfWork.Reposoitory<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 5. Create Order
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var exsitingOrder = await _unitOfWork.Reposoitory<Order>().GetByIdWithSpecAsync(spec);

            if(exsitingOrder != null)
            {
                _unitOfWork.Reposoitory<Order>().Delete(exsitingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal , basket.PaymentIntentId);
            //await _orderRepo.CreateAsync(order);
            await _unitOfWork.Reposoitory<Order>().CreateAsync(order);

            // 6. Save To Database [TODO]
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliverMethods = await _unitOfWork.Reposoitory<DeliveryMethod>().GetAllAsync();
            return deliverMethods;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int OrderId, string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveyMethodSpecification(OrderId , buyerEmail);
            var order = await _unitOfWork.Reposoitory<Order>().GetByIdWithSpecAsync(spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveyMethodSpecification(buyerEmail);
            var orders = await _unitOfWork.Reposoitory<Order>().GetAllwithSpecAsync(spec);
            
            return orders;
        }
    }
}
