using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTeaECommerce.DataAccess.Migrations
{
    public partial class changeOnDeleteSetNullToRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryDetails_Deliveries",
                table: "DeliveryDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_OrderHeader",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeader_ApplicationUser",
                table: "OrderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shops",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_ApplicationUsers",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Products",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_ApplicationUsers",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Products",
                table: "ShoppingCarts");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryDetails_Deliveries",
                table: "DeliveryDetails",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_OrderHeader",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeader_ApplicationUser",
                table: "OrderHeaders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shops",
                table: "Products",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_ApplicationUsers",
                table: "Ratings",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Products",
                table: "Ratings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_ApplicationUsers",
                table: "ShoppingCarts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Products",
                table: "ShoppingCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryDetails_Deliveries",
                table: "DeliveryDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_OrderHeader",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeader_ApplicationUser",
                table: "OrderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shops",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_ApplicationUsers",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Products",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_ApplicationUsers",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Products",
                table: "ShoppingCarts");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryDetails_Deliveries",
                table: "DeliveryDetails",
                column: "DeliveryId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_OrderHeader",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeader_ApplicationUser",
                table: "OrderHeaders",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shops",
                table: "Products",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_ApplicationUsers",
                table: "Ratings",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Products",
                table: "Ratings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_ApplicationUsers",
                table: "ShoppingCarts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Products",
                table: "ShoppingCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
