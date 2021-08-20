using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramAspBot.Migrations
{
    public partial class ReworkItemBackpackItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Durability",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "DurabilityDenominator",
                table: "Items",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsForShopSale",
                table: "Items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxTemperature",
                table: "FishReferences",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinTemperature",
                table: "FishReferences",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WeatherType",
                table: "FishReferences",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FishType",
                table: "FishPediaInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FishPediaInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rarity",
                table: "FishPediaInfos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "CatchBonus",
                table: "BackpackItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DurabilityDenominator",
                table: "BackpackItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "FishBiteType",
                table: "BackpackItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "BackpackItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HookBonus",
                table: "BackpackItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsEvent",
                table: "BackpackItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PlayerPrice",
                table: "BackpackItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopPrice",
                table: "BackpackItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "BackpackItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durability",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DurabilityDenominator",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsForShopSale",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "MaxTemperature",
                table: "FishReferences");

            migrationBuilder.DropColumn(
                name: "MinTemperature",
                table: "FishReferences");

            migrationBuilder.DropColumn(
                name: "WeatherType",
                table: "FishReferences");

            migrationBuilder.DropColumn(
                name: "FishType",
                table: "FishPediaInfos");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FishPediaInfos");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "FishPediaInfos");

            migrationBuilder.DropColumn(
                name: "CatchBonus",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "DurabilityDenominator",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "FishBiteType",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "HookBonus",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "IsEvent",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "PlayerPrice",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "ShopPrice",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "BackpackItems");

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
    }
}
