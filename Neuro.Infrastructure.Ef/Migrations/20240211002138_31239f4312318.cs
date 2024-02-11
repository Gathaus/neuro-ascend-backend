using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _31239f4312318 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationTimes_UserMedicines_UserMedicineId",
                table: "MedicationTimes");

            migrationBuilder.AlterColumn<int>(
                name: "UserMedicineId",
                table: "MedicationTimes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationTimes_UserMedicines_UserMedicineId",
                table: "MedicationTimes",
                column: "UserMedicineId",
                principalTable: "UserMedicines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationTimes_UserMedicines_UserMedicineId",
                table: "MedicationTimes");

            migrationBuilder.AlterColumn<int>(
                name: "UserMedicineId",
                table: "MedicationTimes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationTimes_UserMedicines_UserMedicineId",
                table: "MedicationTimes",
                column: "UserMedicineId",
                principalTable: "UserMedicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
