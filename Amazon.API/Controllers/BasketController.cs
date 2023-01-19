using Amazon.API.Dtos;
using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amazon.API.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepo ,IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }

        // Get : /api/basket?id=basket1
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepo.GetBasketByIdAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        // post : /api/basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedCustomer = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var updatedOrCreatedBasket = await _basketRepo.UpdateBasketAsync(mappedCustomer);
            return Ok(updatedOrCreatedBasket);
        }

        // Delete : /api/basket
        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepo.DeleteBasketAsync(id);
        }
    }
}
