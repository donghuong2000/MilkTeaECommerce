using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTeaECommerce.DataAccess.Migrations
{
    public partial class andshipperToOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipperId",
                table: "OrderDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ShipperId",
                table: "OrderDetails",
                column: "ShipperId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_AspNetUsers_ShipperId",
                table: "OrderDetails",
                column: "ShipperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_AspNetUsers_ShipperId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ShipperId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ShipperId",
                table: "OrderDetails");
        }
    }
}
