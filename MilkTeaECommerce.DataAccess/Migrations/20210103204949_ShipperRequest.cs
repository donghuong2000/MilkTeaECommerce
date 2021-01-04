using Microsoft.EntityFrameworkCore.Migrations;

namespace MilkTeaECommerce.DataAccess.Migrations
{
    public partial class ShipperRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipperRequest",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipperRequest",
                table: "AspNetUsers");
        }
    }
}
