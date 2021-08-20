using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TelegramAspBot.Migrations
{
    public partial class FishStats3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackpackItems_FishStats_FishStatId",
                table: "BackpackItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_FishStats_FishStatId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "FishStats");

            migrationBuilder.DropIndex(
                name: "IX_Items_FishStatId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_BackpackItems_FishStatId",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "FishStatId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FishStatId",
                table: "BackpackItems");

            migrationBuilder.AddColumn<int>(
                name: "CurrentExp",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FishLevel",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Power",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToNextLvlExp",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentExp",
                table: "BackpackItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FishLevel",
                table: "BackpackItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Power",
                table: "BackpackItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToNextLvlExp",
                table: "BackpackItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentExp",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FishLevel",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ToNextLvlExp",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CurrentExp",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "FishLevel",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "BackpackItems");

            migrationBuilder.DropColumn(
                name: "ToNextLvlExp",
                table: "BackpackItems");

            migrationBuilder.AddColumn<int>(
                name: "FishStatId",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FishStatId",
                table: "BackpackItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FishStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrentExp = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Power = table.Column<int>(type: "integer", nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    ToNextLvlExp = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishStats", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_FishStatId",
                table: "Items",
                column: "FishStatId");

            migrationBuilder.CreateIndex(
                name: "IX_BackpackItems_FishStatId",
                table: "BackpackItems",
                column: "FishStatId");

            migrationBuilder.AddForeignKey(
                name: "FK_BackpackItems_FishStats_FishStatId",
                table: "BackpackItems",
                column: "FishStatId",
                principalTable: "FishStats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_FishStats_FishStatId",
                table: "Items",
                column: "FishStatId",
                principalTable: "FishStats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
