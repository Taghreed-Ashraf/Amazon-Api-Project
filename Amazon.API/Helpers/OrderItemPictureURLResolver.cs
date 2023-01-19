using Amazon.API.Dtos;
using Amazon.Core.Entities;
using Amazon.Core.Entities.Order_Aggregate;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Amazon.API.Helpers
{
    public class OrderItemPictureURLResolver : IValueResolver<OrderItem, OrderItemToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureURLResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            return null;
        }
    }
}
