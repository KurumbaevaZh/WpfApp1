using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class models
    {
        public class Client
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string SurName { get; set; }
            public virtual ICollection<Order> Orders { get; set; }
        }

        public class Service
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public virtual ICollection<OrderItem> OrderItems { get; set; }
        }

        public class Order
        {
            public int OrderId { get; set; }
            public int ClientId { get; set; }
            public string ClientName { get; set; }
            public DateTime OrderDate { get; set; }
            public virtual Client Client { get; set; }
            public string Status { get; set; }
            public virtual ICollection<OrderItem> OrderItems { get; set; }
        }

        public class OrderItem
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public int ServiceId { get; set; }
            public virtual Order Order { get; set; }
            public virtual Service Service { get; set; }
        }

        public class Request
        {
            public int Id { get; set; }
            public int ClientId { get; set; }
            public string SerialNumber { get; set; }
            public virtual Client Client { get; set; }
        }

    }
}
