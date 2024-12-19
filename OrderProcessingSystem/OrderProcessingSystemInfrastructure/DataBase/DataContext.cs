using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using OrderProcessingSystemInfrastructure.Repositories.UserRepo;


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
            

            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            
            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<OrderEntity>()
                .HasKey(o => o.Id); 

            modelBuilder.Entity<OrderEntity>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();
            // Configure OrderEntity and UserEntity relationship
            modelBuilder.Entity<UserEntity>()
                 .HasMany(u => u.UserClaims)
                 .WithOne()
                 .HasForeignKey(uc => uc.UserId)
                 .IsRequired();

            modelBuilder.Entity<OrderEntity>()
                .HasOne(o => o.Customer) // Order has one Customer
                .WithMany(u => u.Orders) // Customer has many Orders
                .HasForeignKey(o => o.CustomerId) // Foreign key is CustomerId
                .OnDelete(DeleteBehavior.Cascade); // Define delete behavior

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
                    NormalizedUserName = "CUSTOMER1", // Ensure this is normalized
                    Email = "customer1@example.com",
                    NormalizedEmail = "CUSTOMER1@EXAMPLE.COM", // Ensure this is normalized
                    EmailConfirmed = true,
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = true,
                    IsCustomer = true,
                    PasswordHash = new PasswordHasher<UserEntity>().HashPassword(null, "password123"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    LockoutEnd = null,
                },
                new UserEntity
                {
                    Id = 2,
                    UserName = "customer2",
                    NormalizedUserName = "CUSTOMER2",
                    Email = "customer2@example.com",
                    NormalizedEmail = "CUSTOMER2@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "9876543210",
                    PhoneNumberConfirmed = true,
                    IsCustomer = true,
                    PasswordHash = new PasswordHasher<UserEntity>().HashPassword(null, "password123"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    LockoutEnd = null,
                },
                new UserEntity
                {
                    Id = 3,
                    UserName = "admin1",
                    NormalizedUserName = "ADMIN1",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "5555555555",
                    PhoneNumberConfirmed = true,
                    IsCustomer = false,
                    PasswordHash = new PasswordHasher<UserEntity>().HashPassword(null, "adminpassword"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    LockoutEnd = null,
                }
            );

            // Seeding Roles
            modelBuilder.Entity<RoleEntity>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "Customer", NormalizedName = "CUSTOMER" }
            );

            // Seeding UserRoles (Assigning roles to users)
            modelBuilder.Entity<UserRoleEntity>().HasData(
                new UserRoleEntity { UserId = 1, RoleId = 2 }, // customer1 is a Customer
                new UserRoleEntity { UserId = 2, RoleId = 2 }, // customer2 is a Customer
                new UserRoleEntity { UserId = 3, RoleId = 1 }  // admin1 is an Admin
            );

            // Seeding User Claims with explicit negative Ids
            modelBuilder.Entity<IdentityUserClaim<int>>().HasData(
                new IdentityUserClaim<int> { Id = 1, UserId = 1, ClaimType = "IsCustomer", ClaimValue = "true" },
                new IdentityUserClaim<int> { Id = 2, UserId = 2, ClaimType = "IsCustomer", ClaimValue = "true" },
                new IdentityUserClaim<int> { Id = 3, UserId = 3, ClaimType = "IsCustomer", ClaimValue = "false" },
                new IdentityUserClaim<int> { Id = 4, UserId = 1, ClaimType = "CanPlaceOrder", ClaimValue = "true" },
                new IdentityUserClaim<int> { Id = 5, UserId = 2, ClaimType = "CanPlaceOrder", ClaimValue = "true" },
                new IdentityUserClaim<int> { Id = 6, UserId = 3, ClaimType = "CanPlaceOrder", ClaimValue = "false" }
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
