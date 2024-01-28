using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _31gasdgeeadf98675311InitialC1reate123123333131221213129 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicineUsers");

            migrationBuilder.DropColumn(
                name: "IsTaken",
                table: "UserMedicines");

            migrationBuilder.AddColumn<int>(
                name: "MedicationId",
                table: "UserMedicines",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicationDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: true),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    UserMedicineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationDays_UserMedicines_UserMedicineId",
                        column: x => x.UserMedicineId,
                        principalTable: "UserMedicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimesOfDay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    UserMedicineId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesOfDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimesOfDay_UserMedicines_UserMedicineId",
                        column: x => x.UserMedicineId,
                        principalTable: "UserMedicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMedicines_MedicationId",
                table: "UserMedicines",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedicines_UserId",
                table: "UserMedicines",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationDays_UserMedicineId",
                table: "MedicationDays",
                column: "UserMedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesOfDay_UserMedicineId",
                table: "TimesOfDay",
                column: "UserMedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMedicines_Medications_MedicationId",
                table: "UserMedicines",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMedicines_Users_UserId",
                table: "UserMedicines",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMedicines_Medications_MedicationId",
                table: "UserMedicines");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMedicines_Users_UserId",
                table: "UserMedicines");

            migrationBuilder.DropTable(
                name: "MedicationDays");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "TimesOfDay");

            migrationBuilder.DropIndex(
                name: "IX_UserMedicines_MedicationId",
                table: "UserMedicines");

            migrationBuilder.DropIndex(
                name: "IX_UserMedicines_UserId",
                table: "UserMedicines");

            migrationBuilder.DropColumn(
                name: "MedicationId",
                table: "UserMedicines");

            migrationBuilder.AddColumn<bool>(
                name: "IsTaken",
                table: "UserMedicines",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MedicineUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    WeekDay = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineUsers", x => x.Id);
                });
        }
    }
}
