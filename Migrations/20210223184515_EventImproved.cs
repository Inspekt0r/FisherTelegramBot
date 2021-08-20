using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramAspBot.Migrations
{
    public partial class EventImproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEvent",
                table: "Items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEvent",
                table: "FishReferences",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEvent",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsEvent",
                table: "FishReferences");
        }
    }
}
