﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramAspBot.Migrations
{
    public partial class EventTimer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FirstEventFishCatchTime",
                table: "CharStat",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstEventFishCatchTime",
                table: "CharStat");
        }
    }
}