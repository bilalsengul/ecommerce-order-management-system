using System;
using System.Collections.Generic;

namespace ECommerceOrderManagement.Core.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Product()
        {
            Id = Guid.NewGuid();
            OrderItems = new List<OrderItem>();
        }
    }
} 