using Amazon.API.Dtos;
using Amazon.API.Errors;
using Amazon.API.Helpers;
using Amazon.Core.Entities.Order_Aggregate;
using Amazon.Core.Services;
using Amazon.Core.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Amazon.API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;

        public OrdersController(IOrderServices orderServices , IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }

        // POST : /api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orderAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderServices.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, orderAddress);
            if (order == null)
                return BadRequest( new ApiResponse(400));

            return Ok(_mapper.Map<Order , OrderToReturnDto> (order));
        }


        // GET : /api/Orders
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUsers()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderServices.GetOrdersForUserAsync(buyerEmail);
            return Ok(_mapper.Map<IReadOnlyList<Order> , IReadOnlyList<OrderToReturnDto>> (orders));
        }

        // Get : /api/Orders/10
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderServices.GetOrderByIdForUserAsync(id, buyerEmail);

            if (order == null)
                return BadRequest(new ApiResponse(400));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        // GET : /api/Orders/deliveryMethods
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderServices.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
    }
}
