using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystemInfrastructure.DataBase.Entities;


namespace OrderProcessingSystemInfrastructure.DataBase
{
    public class DataBaseContext : IdentityDbContext<UserEntity, IdentityRole<int>, int>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }
        public DbSet<IdentityRole<int>> Roles { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderProductEntity> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderEntity>().HasKey(o => o.Id);
            // Configure OrderEntity and UserEntity relationship
            modelBuilder.Entity<OrderEntity>()
                .HasOne(o => o.Customer) // Order has one Customer
                .WithMany(u => u.Orders) // Customer has many Orders
                .HasForeignKey(o => o.CustomerId) // Foreign key is CustomerId
                .OnDelete(DeleteBehavior.Cascade); // Optional: Define delete behavior

            modelBuilder.Entity<ProductEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<OrderProductEntity>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            // Configuring the many-to-many relationship between Orders and Products
            modelBuilder.Entity<OrderProductEntity>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProductEntity>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);
            // Configure UserEntity
            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.Orders) // User has many Orders
                .WithOne(o => o.Customer) // Order has one Customer
                .HasForeignKey(o => o.CustomerId); // Foreign key is CustomerId

            // Seeding data
            DataSedding(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<OrderEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    var order = entry.Entity;

                    bool hasUnfulfilledOrders = await Orders
                                                      .AnyAsync(o => o.CustomerId == order.CustomerId && !o.IsFulfilled);

                    if (hasUnfulfilledOrders)// db validation for unfullfilled orders
                    {
                        throw new InvalidOperationException(TextMessages.CanNotPlaceOrderUnfulfilledOrder);
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        private static void DataSedding(ModelBuilder modelBuilder)
        {
            // Seeding products
            modelBuilder.Entity<ProductEntity>().HasData(
                new ProductEntity { Id = 1, Name = "Laptop", Price = 1200.00m },
                new ProductEntity { Id = 2, Name = "Smartphone", Price = 800.00m },
                new ProductEntity { Id = 3, Name = "Headphones", Price = 150.00m }
            );

            // Seeding users
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    UserName = "customer1",
                    Email = "customer1@example.com",
                    IsCustomer = true
                },
                new UserEntity
                {
                    Id = 2,
                    UserName = "customer2",
                    Email = "customer2@example.com",
                    IsCustomer = true
                },
                new UserEntity
                {
                    Id = 3,
                    UserName = "admin1",
                    Email = "admin@example.com",
                    IsCustomer = false
                }
            );

            // Seeding Orders
            modelBuilder.Entity<OrderEntity>().HasData(
                new OrderEntity { Id = 1, CustomerId = 1, IsFulfilled = true },
                new OrderEntity { Id = 2, CustomerId = 2, IsFulfilled = false }
            );

            // Seeding OrderProduct Join Table
            modelBuilder.Entity<OrderProductEntity>().HasData(
                new OrderProductEntity { OrderId = 1, ProductId = 1, Quantity = 1 },
                new OrderProductEntity { OrderId = 1, ProductId = 3, Quantity = 2 },
                new OrderProductEntity { OrderId = 2, ProductId = 2, Quantity = 2 },
                new OrderProductEntity { OrderId = 2, ProductId = 3, Quantity = 4 }
            );
        }

    }

}
