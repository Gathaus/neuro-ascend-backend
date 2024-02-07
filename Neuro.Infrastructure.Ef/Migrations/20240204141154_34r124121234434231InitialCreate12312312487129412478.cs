﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _34r124121234434231InitialCreate12312312487129412478 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "TimesOfDay",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Time",
                table: "TimesOfDay",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
