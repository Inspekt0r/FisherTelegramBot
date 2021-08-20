using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramAspBot.Migrations
{
    public partial class PersonalAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendPersonalAlert",
                table: "Characters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendPersonalAlert",
                table: "Characters");
        }
    }
}
