using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _1531gasdgeeadf98675311InitialC1reate123123333131221213129 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BeginDay",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BeginMonth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BeginningDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DiseaseTerm",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EndDay",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EndMonth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReminderTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SelectedDays",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Usage",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "View",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "DiseaseLevel",
                table: "Users",
                newName: "AlzheimerStage");

            migrationBuilder.AddColumn<DateOnly>(
                name: "BeginningDate",
                table: "UserMedicines",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "UserMedicines",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "PillNumber",
                table: "UserMedicines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Usage",
                table: "UserMedicines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diseases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Diseases_UserId",
                table: "Diseases",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropColumn(
                name: "BeginningDate",
                table: "UserMedicines");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "UserMedicines");

            migrationBuilder.DropColumn(
                name: "PillNumber",
                table: "UserMedicines");

            migrationBuilder.DropColumn(
                name: "Usage",
                table: "UserMedicines");

            migrationBuilder.RenameColumn(
                name: "AlzheimerStage",
                table: "Users",
                newName: "DiseaseLevel");

            migrationBuilder.AddColumn<string>(
                name: "Amount",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BeginDay",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BeginMonth",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "BeginningDate",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "DiseaseTerm",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "EndDay",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EndMonth",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReminderTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<byte[]>(
                name: "SelectedDays",
                table: "Users",
                type: "smallint[]",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Usage",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "View",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
