using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramAspBot.Migrations
{
    public partial class ReworkItemBackpackItem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FishId",
                table: "FishPediaInfos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FishPediaInfos_FishId",
                table: "FishPediaInfos",
                column: "FishId");

            migrationBuilder.AddForeignKey(
                name: "FK_FishPediaInfos_Items_FishId",
                table: "FishPediaInfos",
                column: "FishId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FishPediaInfos_Items_FishId",
                table: "FishPediaInfos");

            migrationBuilder.DropIndex(
                name: "IX_FishPediaInfos_FishId",
                table: "FishPediaInfos");

            migrationBuilder.DropColumn(
                name: "FishId",
                table: "FishPediaInfos");
        }
    }
}
