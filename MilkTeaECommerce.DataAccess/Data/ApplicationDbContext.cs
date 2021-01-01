using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Data
{
    public   class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }
        public  DbSet<ApplicationUser> AspNetUsers { get; set; }
        public   DbSet<Category> Categories { get; set; }
        public  DbSet<CategoryDiscount> CategoryDiscount { get; set; }
        public  DbSet<Delivery> Deliveries { get; set; }
        public  DbSet<DeliveryDetail> DeliveryDetails { get; set; }
        public  DbSet<DeliveryDiscount> DeliveryDiscount { get; set; }
        public  DbSet<Discount> Discounts { get; set; }
        public  DbSet<OrderDetail> OrderDetails { get; set; }
        public  DbSet<OrderHeader> OrderHeaders { get; set; }
        public  DbSet<ProductDiscount> ProductDiscount { get; set; }
        public  DbSet<Product> Products { get; set; }
        public  DbSet<Rating> Ratings { get; set; }
        public  DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public  DbSet<Shop> Shops { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<CategoryDiscount>(entity =>
            {
                entity.HasKey(e => new { e.DiscountId, e.CategoryId })
                    .HasName("PK__Category__45AFFE36615A770A");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryDiscount)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategoryDiscount_Category");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.CategoryDiscount)
                    .HasForeignKey(d => d.DiscountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CategoryDiscount_Discount");
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<DeliveryDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("PK__Delivery__D3B9D36CF25242FD");

                entity.Property(e => e.DateEnd).HasColumnType("date");

                entity.Property(e => e.DateStart).HasColumnType("date");

                entity.Property(e => e.DeliveryId).HasMaxLength(450);

                entity.HasOne(d => d.Delivery)
                    .WithMany(p => p.DeliveryDetails)
                    .HasForeignKey(d => d.DeliveryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DeliveryDetails_Deliveries");

                entity.HasOne(d => d.OrderDetail)
                    .WithOne(p => p.DeliveryDetails)
                    .HasForeignKey<DeliveryDetail>(d => d.OrderDetailId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DeliveryDetails_OrderHeaders");
            });

            modelBuilder.Entity<DeliveryDiscount>(entity =>
            {
                entity.HasKey(e => new { e.DiscountId, e.DeliveryId })
                    .HasName("PK__Delivery__1219B56A28A08CCD");

                entity.HasOne(d => d.Delivery)
                    .WithMany(p => p.DeliveryDiscount)
                    .HasForeignKey(d => d.DeliveryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DeliveryDiscount_Deliveries");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.DeliveryDiscount)
                    .HasForeignKey(d => d.DiscountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DeliveryDiscount_Discount");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasIndex(e => e.Code)
                    .HasName("UQ__Discount__A25C5AA78CED2B9A")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DateExpired).HasColumnType("date");

                entity.Property(e => e.DateStart).HasColumnType("date");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.OrderHeaderId).HasMaxLength(450);

                entity.Property(e => e.ProductId).HasMaxLength(450);

                entity.HasOne(d => d.OrderHeader)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderHeaderId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_OrderDetail_OrderHeader");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<OrderHeader>(entity =>
            {
                entity.Property(e => e.ApplicationUserId).HasMaxLength(450);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.OrderHeaders)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_OrderHeader_ApplicationUser");
            });

            modelBuilder.Entity<ProductDiscount>(entity =>
            {
                entity.HasKey(e => new { e.DiscountId, e.ProductId })
                    .HasName("PK__ProductD__2F7FA1FAC61561F2");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.ProductDiscount)
                    .HasForeignKey(d => d.DiscountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProductDiscount_Discount");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDiscount)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProductDiscount_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.CategoryId).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.ShopId).HasMaxLength(450);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_Shops");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.Property(e => e.ApplicationUserId).HasMaxLength(450);

                entity.Property(e => e.Content).HasMaxLength(450);

                entity.Property(e => e.ProductId).HasMaxLength(450);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ratings_ApplicationUsers");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ratings_Products");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.Property(e => e.ApplicationUserId).HasMaxLength(450);

                entity.Property(e => e.ProductId).HasMaxLength(450);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ShoppingCarts_ApplicationUsers");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ShoppingCarts_Products");
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.ApplicationUserId)
                    .HasName("PK__Shops__9CBCE319A2F8A378");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.ApplicationUser)
                    .WithOne(p => p.Shops)
                    .HasForeignKey<Shop>(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SHOP_USER");
            });
            base.OnModelCreating(modelBuilder);
        }

        
    }
}
