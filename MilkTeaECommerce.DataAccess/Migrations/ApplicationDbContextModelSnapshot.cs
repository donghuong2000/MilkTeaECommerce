﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MilkTeaECommerce.Data;

namespace MilkTeaECommerce.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.CategoryDiscount", b =>
                {
                    b.Property<string>("DiscountId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DiscountId", "CategoryId")
                        .HasName("PK__Category__45AFFE36615A770A");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryDiscount");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.Delivery", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.DeliveryDetail", b =>
                {
                    b.Property<string>("OrderDetailId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateEnd")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DateStart")
                        .HasColumnType("date");

                    b.Property<string>("DeliveryId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Price")
                        .HasColumnType("real");

                    b.HasKey("OrderDetailId")
                        .HasName("PK__Delivery__D3B9D36CF25242FD");

                    b.HasIndex("DeliveryId");

                    b.ToTable("DeliveryDetails");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.DeliveryDiscount", b =>
                {
                    b.Property<string>("DiscountId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeliveryId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DiscountId", "DeliveryId")
                        .HasName("PK__Delivery__1219B56A28A08CCD");

                    b.HasIndex("DeliveryId");

                    b.ToTable("DeliveryDiscount");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.Discount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("DateExpired")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DateStart")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("MaxDiscount")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PercentDiscount")
                        .HasColumnType("int");

                    b.Property<int?>("TimesUseLimit")
                        .HasColumnType("int");

                    b.Property<int?>("TimesUsed")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasName("UQ__Discount__A25C5AA78CED2B9A");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.OrderDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Count")
                        .HasColumnType("int");

                    b.Property<string>("OrderHeaderId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<float?>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrderHeaderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.OrderHeader", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("PaymentStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("OrderHeaders");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<float?>("Price")
                        .HasColumnType("real");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("ShopId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ShopId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.ProductDiscount", b =>
                {
                    b.Property<string>("DiscountId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DiscountId", "ProductId")
                        .HasName("PK__ProductD__2F7FA1FAC61561F2");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductDiscount");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.Rating", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<float?>("Rate")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ProductId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.Shop", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<float?>("Rate")
                        .HasColumnType("real");

                    b.HasKey("ApplicationUserId")
                        .HasName("PK__Shops__9CBCE319A2F8A378");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.ShoppingCart", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.Property<int?>("Count")
                        .HasColumnType("int");

                    b.Property<float?>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ProductId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.CategoryDiscount", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.Category", "Category")
                        .WithMany("CategoryDiscount")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_CategoryDiscount_Category")
                        .IsRequired();

                    b.HasOne("MilkTeaECommerce.Models.Discount", "Discount")
                        .WithMany("CategoryDiscount")
                        .HasForeignKey("DiscountId")
                        .HasConstraintName("FK_CategoryDiscount_Discount")
                        .IsRequired();
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.DeliveryDetail", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.Delivery", "Delivery")
                        .WithMany("DeliveryDetails")
                        .HasForeignKey("DeliveryId")
                        .HasConstraintName("FK_DeliveryDetails_Deliveries")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MilkTeaECommerce.Models.OrderDetail", "OrderDetail")
                        .WithOne("DeliveryDetails")
                        .HasForeignKey("MilkTeaECommerce.Models.DeliveryDetail", "OrderDetailId")
                        .HasConstraintName("FK_DeliveryDetails_OrderHeaders")
                        .IsRequired();
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.DeliveryDiscount", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.Delivery", "Delivery")
                        .WithMany("DeliveryDiscount")
                        .HasForeignKey("DeliveryId")
                        .HasConstraintName("FK_DeliveryDiscount_Deliveries")
                        .IsRequired();

                    b.HasOne("MilkTeaECommerce.Models.Discount", "Discount")
                        .WithMany("DeliveryDiscount")
                        .HasForeignKey("DiscountId")
                        .HasConstraintName("FK_DeliveryDiscount_Discount")
                        .IsRequired();
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.OrderDetail", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.OrderHeader", "OrderHeader")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderHeaderId")
                        .HasConstraintName("FK_OrderDetail_OrderHeader")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MilkTeaECommerce.Models.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_OrderDetail_Product")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.OrderHeader", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("OrderHeaders")
                        .HasForeignKey("ApplicationUserId")
                        .HasConstraintName("FK_OrderHeader_ApplicationUser")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.Product", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_Products_Categories")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MilkTeaECommerce.Models.Shop", "Shop")
                        .WithMany("Products")
                        .HasForeignKey("ShopId")
                        .HasConstraintName("FK_Products_Shops")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.ProductDiscount", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.Discount", "Discount")
                        .WithMany("ProductDiscount")
                        .HasForeignKey("DiscountId")
                        .HasConstraintName("FK_ProductDiscount_Discount")
                        .IsRequired();

                    b.HasOne("MilkTeaECommerce.Models.Product", "Product")
                        .WithMany("ProductDiscount")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_ProductDiscount_Product")
                        .IsRequired();
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.Rating", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Ratings")
                        .HasForeignKey("ApplicationUserId")
                        .HasConstraintName("FK_Ratings_ApplicationUsers")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MilkTeaECommerce.Models.Product", "Product")
                        .WithMany("Ratings")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_Ratings_Products")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.Shop", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("Shops")
                        .HasForeignKey("MilkTeaECommerce.Models.Shop", "ApplicationUserId")
                        .HasConstraintName("FK_SHOP_USER")
                        .IsRequired();
                });

            modelBuilder.Entity("MilkTeaECommerce.Models.ShoppingCart", b =>
                {
                    b.HasOne("MilkTeaECommerce.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("ShoppingCarts")
                        .HasForeignKey("ApplicationUserId")
                        .HasConstraintName("FK_ShoppingCarts_ApplicationUsers")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MilkTeaECommerce.Models.Product", "Product")
                        .WithMany("ShoppingCarts")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_ShoppingCarts_Products")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
