using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramAspBot.Migrations
{
    public partial class RemoVeSpotFromFishRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spots_FishReferences_FishReferenceId",
                table: "Spots");

            migrationBuilder.DropIndex(
                name: "IX_Spots_FishReferenceId",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "FishReferenceId",
                table: "Spots");

            migrationBuilder.AddColumn<double>(
                name: "MaxTemperature",
                table: "Items",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinTemperature",
                table: "Items",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "WeatherType",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxTemperature",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "MinTemperature",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WeatherType",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "FishReferenceId",
                table: "Spots",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spots_FishReferenceId",
                table: "Spots",
                column: "FishReferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Spots_FishReferences_FishReferenceId",
                table: "Spots",
                column: "FishReferenceId",
                principalTable: "FishReferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
