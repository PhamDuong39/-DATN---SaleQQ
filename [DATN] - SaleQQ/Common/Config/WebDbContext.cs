using _DATN____SaleQQ.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _DATN____SaleQQ.Common.Config
{
    public class WebDbContext : IdentityDbContext
    {
        public WebDbContext() { }

        public WebDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Cart> cart { get; set; }

        public virtual DbSet<CartItem> cart_item { get; set; }

        public virtual DbSet<Order> order { get; set; }

        public virtual DbSet<OrderDetail> order_detail { get; set; }

        public virtual DbSet<OrderStatus> order_status { get; set; }

        public virtual DbSet<Payment> payment { get; set; }

        public virtual DbSet<Product> product { get; set; }

        public virtual DbSet<ProductImage> product_image { get; set; }

        public virtual DbSet<ProductReview> product_review { get; set; }

        public virtual DbSet<ProductType> product_type { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");

            modelBuilder.Entity<Order>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Order>()
               .HasOne<Payment>(p => p.Payment)
               .WithMany(p => p.Orders)
               .HasForeignKey(p => p.PaymentId);

            modelBuilder.Entity<Order>()
              .HasOne<OrderStatus>(p => p.OrderStatus)
              .WithMany(p => p.Orders)
              .HasForeignKey(p => p.OrderStatusId);

            modelBuilder.Entity<ProductReview>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<ProductReview>()
               .HasOne<Product>(p => p.Product)
               .WithMany(p => p.ProductReviews)
               .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Cart>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<CartItem>()
                .HasOne(p => p.Cart)
                .WithMany(p => p.CartItems)
                .HasForeignKey(p => p.CartId);

            modelBuilder.Entity<CartItem>()
               .HasOne(p => p.Product)
               .WithMany(p => p.CartItems)
               .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(p => p.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<OrderDetail>()
               .HasOne(p => p.Product)
               .WithMany(p => p.OrderDetails)
               .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
              .HasOne(p => p.ProductType)
              .WithMany(p => p.Products)
              .HasForeignKey(p => p.ProductTypeId);

            modelBuilder.Entity<ProductImage>()
             .HasOne(p => p.Product)
             .WithMany(p => p.ProductImages)
             .HasForeignKey(p => p.ProductId);

            CreateRoles(modelBuilder);
        }

        protected void CreateRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole() { Name = "Employee", NormalizedName = "EMPLOYEE" },
                new IdentityRole() { Name = "User", NormalizedName = "USER" }
                );
        }
    }
}
