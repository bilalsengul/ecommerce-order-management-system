using System;

namespace ECommerceOrderManagement.Core.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        public OrderItem()
        {
            Id = Guid.NewGuid();
        }
    }
} 