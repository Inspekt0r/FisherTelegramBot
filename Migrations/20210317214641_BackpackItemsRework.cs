using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramAspBot.Migrations
{
    public partial class BackpackItemsRework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "BackpackItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemType",
                table: "BackpackItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rarity",
                table: "BackpackItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "BackpackItems");
        }
    }
}
