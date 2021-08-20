using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TelegramAspBot.Migrations
{
    public partial class CreateItemReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemReferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    CatchBonus = table.Column<double>(type: "double precision", nullable: false),
                    HookBonus = table.Column<double>(type: "double precision", nullable: false),
                    FishBiteType = table.Column<int>(type: "integer", nullable: false),
                    ShopPrice = table.Column<int>(type: "integer", nullable: false),
                    PlayerPrice = table.Column<int>(type: "integer", nullable: false),
                    IsEvent = table.Column<bool>(type: "boolean", nullable: false),
                    EventPosition = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReferences", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemReferences");
        }
    }
}
