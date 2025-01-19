using System;
using System.Collections.Generic;

namespace ECommerceOrderManagement.Core.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            Id = Guid.NewGuid();
            OrderNumber = Guid.NewGuid().ToString();
            OrderDate = DateTime.UtcNow;
            OrderItems = new List<OrderItem>();
        }
    }
} 