using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfApp1.models;
using Microsoft.EntityFrameworkCore; // Это нужно для использования метода SaveChanges()



namespace WpfApp1
{
    internal class NadezhdaaDbContext : DbContext
    {
        public NadezhdaaDbContext(DbContextOptions<NadezhdaaDbContext> options)
        : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=nadezhdaa");
        }
        public NadezhdaaDbContext()
        {
        }
        public class Order
        {
            public int OrderId { get; set; }
            public int ClientId { get; set; }
            public string Name { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public Client Client { get; set; } // Связь с клиентом
        }

        public class Service
        {
            public int ServiceId { get; set; }
            public string ServiceName { get; set; }
            public decimal Price { get; set; }
        }
        public class User
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
        }
        public class OrderItem
        {
            public int OrderItemId { get; set; }
            public int OrderId { get; set; }
            public Order Order { get; set; }
            public int ServiceId { get; set; }
            public Service Service { get; set; }
            public int Quantity { get; set; }
        }

        public class Client
        {
            public int ClientId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
        }


        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ReportsWindow> Report { get; set; }
        public DbSet<Report> Reports { get; set; }

        public void SaveChanges()
        {
        }
        
    }

}
