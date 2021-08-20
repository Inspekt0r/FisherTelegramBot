using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TelegramAspBot.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Backpacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backpacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharStat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FishCaughtCount = table.Column<int>(type: "integer", nullable: false),
                    HookCount = table.Column<int>(type: "integer", nullable: false),
                    MostWeightFish = table.Column<double>(type: "double precision", nullable: false),
                    MostHeightFish = table.Column<double>(type: "double precision", nullable: false),
                    FishingTry = table.Column<int>(type: "integer", nullable: false),
                    BaitUsingCount = table.Column<int>(type: "integer", nullable: false),
                    UnluckyTry = table.Column<int>(type: "integer", nullable: false),
                    FishOfMyDreams = table.Column<int>(type: "integer", nullable: false),
                    Percent = table.Column<int>(type: "integer", nullable: false),
                    SeasonFishCaughtCount = table.Column<int>(type: "integer", nullable: false),
                    SeasonHookCount = table.Column<int>(type: "integer", nullable: false),
                    SeasonMostWeightFish = table.Column<double>(type: "double precision", nullable: false),
                    SeasonMostWeightName = table.Column<string>(type: "text", nullable: true),
                    SeasonMostHeightFish = table.Column<double>(type: "double precision", nullable: false),
                    SeasonMostHeighеName = table.Column<string>(type: "text", nullable: true),
                    SeasonFishingTry = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharStat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishPedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishPedias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FishReferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaxWeight = table.Column<double>(type: "double precision", nullable: false),
                    MaxHeight = table.Column<double>(type: "double precision", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Rarity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishReferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GlobalSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LureTimeMinutes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    Height = table.Column<double>(type: "double precision", nullable: false),
                    Rarity = table.Column<int>(type: "integer", nullable: false),
                    CatchBonus = table.Column<double>(type: "double precision", nullable: false),
                    HookBonus = table.Column<double>(type: "double precision", nullable: false),
                    FishBiteType = table.Column<int>(type: "integer", nullable: false),
                    ShopPrice = table.Column<int>(type: "integer", nullable: false),
                    PlayerPrice = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeasonStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    IsEnded = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Temperature = table.Column<double>(type: "double precision", nullable: false),
                    WeatherType = table.Column<int>(type: "integer", nullable: false),
                    CatchBonus = table.Column<double>(type: "double precision", nullable: false),
                    FishReferenceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spots_FishReferences_FishReferenceId",
                        column: x => x.FishReferenceId,
                        principalTable: "FishReferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BackpackItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: true),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    IsEquipped = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Durability = table.Column<int>(type: "integer", nullable: false),
                    TimeElapsed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    BackpackId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackpackItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackpackItems_Backpacks_BackpackId",
                        column: x => x.BackpackId,
                        principalTable: "Backpacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BackpackItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FishPediaInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FishId = table.Column<int>(type: "integer", nullable: true),
                    Caught = table.Column<int>(type: "integer", nullable: false),
                    Seen = table.Column<int>(type: "integer", nullable: false),
                    MaxWeight = table.Column<double>(type: "double precision", nullable: false),
                    MaxHeight = table.Column<double>(type: "double precision", nullable: false),
                    FishPediaId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishPediaInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FishPediaInfos_FishPedias_FishPediaId",
                        column: x => x.FishPediaId,
                        principalTable: "FishPedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FishPediaInfos_Items_FishId",
                        column: x => x.FishId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TelegramId = table.Column<int>(type: "integer", nullable: false),
                    BackpackId = table.Column<int>(type: "integer", nullable: true),
                    HealthNow = table.Column<int>(type: "integer", nullable: false),
                    HealthAll = table.Column<int>(type: "integer", nullable: false),
                    Attack = table.Column<int>(type: "integer", nullable: false),
                    Defense = table.Column<int>(type: "integer", nullable: false),
                    CharState = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ActionTimer = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsSetupNickname = table.Column<bool>(type: "boolean", nullable: false),
                    FishingSessionGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CharStatId = table.Column<int>(type: "integer", nullable: true),
                    FishPediaId = table.Column<int>(type: "integer", nullable: true),
                    Money = table.Column<int>(type: "integer", nullable: false),
                    Banned = table.Column<bool>(type: "boolean", nullable: false),
                    HasStartedLure = table.Column<bool>(type: "boolean", nullable: false),
                    SpotId = table.Column<int>(type: "integer", nullable: true),
                    SeasonPoints = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Backpacks_BackpackId",
                        column: x => x.BackpackId,
                        principalTable: "Backpacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_CharStat_CharStatId",
                        column: x => x.CharStatId,
                        principalTable: "CharStat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_FishPedias_FishPediaId",
                        column: x => x.FishPediaId,
                        principalTable: "FishPedias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_Spots_SpotId",
                        column: x => x.SpotId,
                        principalTable: "Spots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FishReferenceSpots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FishReferenceId = table.Column<int>(type: "integer", nullable: true),
                    SpotId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishReferenceSpots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FishReferenceSpots_FishReferences_FishReferenceId",
                        column: x => x.FishReferenceId,
                        principalTable: "FishReferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FishReferenceSpots_Spots_SpotId",
                        column: x => x.SpotId,
                        principalTable: "Spots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: true),
                    LureTimeLeft = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lures_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lures_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackpackItems_BackpackId",
                table: "BackpackItems",
                column: "BackpackId");

            migrationBuilder.CreateIndex(
                name: "IX_BackpackItems_ItemId",
                table: "BackpackItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_BackpackId",
                table: "Characters",
                column: "BackpackId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharStatId",
                table: "Characters",
                column: "CharStatId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_FishPediaId",
                table: "Characters",
                column: "FishPediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_SpotId",
                table: "Characters",
                column: "SpotId");

            migrationBuilder.CreateIndex(
                name: "IX_FishPediaInfos_FishId",
                table: "FishPediaInfos",
                column: "FishId");

            migrationBuilder.CreateIndex(
                name: "IX_FishPediaInfos_FishPediaId",
                table: "FishPediaInfos",
                column: "FishPediaId");

            migrationBuilder.CreateIndex(
                name: "IX_FishReferenceSpots_FishReferenceId",
                table: "FishReferenceSpots",
                column: "FishReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_FishReferenceSpots_SpotId",
                table: "FishReferenceSpots",
                column: "SpotId");

            migrationBuilder.CreateIndex(
                name: "IX_Lures_CharacterId",
                table: "Lures",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lures_ItemId",
                table: "Lures",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Spots_FishReferenceId",
                table: "Spots",
                column: "FishReferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackpackItems");

            migrationBuilder.DropTable(
                name: "FishPediaInfos");

            migrationBuilder.DropTable(
                name: "FishReferenceSpots");

            migrationBuilder.DropTable(
                name: "GlobalSettings");

            migrationBuilder.DropTable(
                name: "Lures");

            migrationBuilder.DropTable(
                name: "SeasonStats");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Backpacks");

            migrationBuilder.DropTable(
                name: "CharStat");

            migrationBuilder.DropTable(
                name: "FishPedias");

            migrationBuilder.DropTable(
                name: "Spots");

            migrationBuilder.DropTable(
                name: "FishReferences");
        }
    }
}
