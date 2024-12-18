﻿using System.Text.Json.Serialization;

namespace OrderProcessingSystem.Shared.Models
{
    public class OrderProductModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; } //for view purpose
        public int Quantity { get; set; }
        [JsonIgnore]
        public decimal TotalPrice => Price * Quantity;//for view purpose
    }
}