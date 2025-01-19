using System;
using System.Collections.Generic;

namespace ECommerceOrderManagement.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            Orders = new List<Order>();
        }
    }
} 