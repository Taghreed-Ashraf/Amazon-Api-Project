using Amazon.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amazon.API.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }

        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

        public int? DeliveryMethodId { get; set; }
        public decimal shippingPrice { get; set; }
        public string PaymentIntentId { get; set; }

        public string ClientSecret { get; set; }
    }
}
