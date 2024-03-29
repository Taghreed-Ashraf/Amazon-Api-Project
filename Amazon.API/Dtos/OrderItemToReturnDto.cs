﻿using Amazon.Core.Entities.Order_Aggregate;

namespace Amazon.API.Dtos
{
    public class OrderItemToReturnDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}