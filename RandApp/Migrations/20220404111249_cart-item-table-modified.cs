using Microsoft.EntityFrameworkCore.Migrations;

namespace RandApp.Migrations
{
    public partial class cartitemtablemodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedItemColor",
                table: "ShoppingCartItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedItemSize",
                table: "ShoppingCartItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedItemColor",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "SelectedItemSize",
                table: "ShoppingCartItems");
        }
    }
}
