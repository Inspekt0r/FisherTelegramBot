using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramAspBot.Migrations
{
    public partial class LureChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lures_Items_ItemId",
                table: "Lures");

            migrationBuilder.AddForeignKey(
                name: "FK_Lures_BackpackItems_ItemId",
                table: "Lures",
                column: "ItemId",
                principalTable: "BackpackItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lures_BackpackItems_ItemId",
                table: "Lures");

            migrationBuilder.AddForeignKey(
                name: "FK_Lures_Items_ItemId",
                table: "Lures",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
