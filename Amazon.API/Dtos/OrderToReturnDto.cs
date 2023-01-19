using Amazon.Core.Entities.Order_Aggregate;
using System.Collections.Generic;
using System;

namespace Amazon.API.Dtos
{
    public class OrderToReturnDto
    {
        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } 

        public string Status { get; set; } 

        public Address ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemToReturnDto> Items { get; set; } 

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public string PaymentIntenId { get; set; }

    }
}
