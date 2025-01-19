using Microsoft.EntityFrameworkCore;
using ECommerceOrderManagement.Core.Entities;

namespace ECommerceOrderManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).IsRequired();
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(e => e.Status).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(e => e.TotalPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(e => e.StockQuantity).IsRequired();
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
            });

            // Seed initial data
            var product1Id = new Guid("98765432-9876-9876-9876-987654321098");
            var product2Id = Guid.NewGuid();
            var product3Id = Guid.NewGuid();

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = product1Id,
                    Name = "Product 1",
                    Description = "Description 1",
                    Price = 10.99m,
                    StockQuantity = 100
                },
                new Product
                {
                    Id = product2Id,
                    Name = "Product 2",
                    Description = "Description 2",
                    Price = 20.99m,
                    StockQuantity = 50
                },
                new Product
                {
                    Id = product3Id,
                    Name = "Product 3",
                    Description = "Description 3",
                    Price = 30.99m,
                    StockQuantity = 75
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("12345678-1234-1234-1234-123456789012"),
                    Username = "johndoe",
                    Email = "john.doe@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEPxuvpHZd4Sjm1/K7xakWoNDr3XW8qujqpC/rJrYR1PZLJ6VVwbf+BebD0qSt2czaA==",
                    FirstName = "John",
                    LastName = "Doe"
                }
            );
        }
    }
} 