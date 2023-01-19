using AutoMapper;
using Amazon.Core.Entities;
using Amazon.API.Dtos;
using Microsoft.Extensions.Configuration;

namespace Amazon.API.Helpers
{
    public class ProductPictureURLResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureURLResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
            return null;
        }
    }
}
