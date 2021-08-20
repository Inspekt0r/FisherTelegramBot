using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TelegramAspBot.Migrations
{
    public partial class AddEventPositionToItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemReferences");

            migrationBuilder.AddColumn<int>(
                name: "EventPosition",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventPosition",
                table: "Items");

            migrationBuilder.CreateTable(
                name: "ItemReferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CatchBonus = table.Column<double>(type: "double precision", nullable: false),
                    EventPosition = table.Column<int>(type: "integer", nullable: false),
                    FishBiteType = table.Column<int>(type: "integer", nullable: false),
                    HookBonus = table.Column<double>(type: "double precision", nullable: false),
                    IsEvent = table.Column<bool>(type: "boolean", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    PlayerPrice = table.Column<int>(type: "integer", nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    ShopPrice = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReferences", x => x.Id);
                });
        }
    }
}
