using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _1239f4312318 : Migration
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
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekDay",
                table: "MedicationTimes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationTimes_UserMedicines_UserMedicineId",
                table: "MedicationTimes",
                column: "UserMedicineId",
                principalTable: "UserMedicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            
            // Geçici bir sütun ekleyin
            migrationBuilder.AddColumn<string>(
                name: "TempTime",
                table: "MedicationTimes",
                nullable: true);

            // Mevcut 'Time' değerlerini geçici sütuna kopyalayın (Bu adım veri kaybını önlemek için önemlidir)
            migrationBuilder.Sql(
                @"
                UPDATE ""MedicationTimes""
                SET ""TempTime"" = TO_CHAR(""Time"", 'HH24:MI:SS')
                ");

            // Orijinal 'Time' sütununu kaldırın
            migrationBuilder.DropColumn(
                name: "Time",
                table: "MedicationTimes");

            // Geçici sütunu 'Time' olarak yeniden adlandırın ve türünü 'interval'e dönüştürün
            migrationBuilder.RenameColumn(
                name: "TempTime",
                table: "MedicationTimes",
                newName: "Time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time",
                table: "MedicationTimes",
                type: "interval",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true,
                defaultValueSql: "'00:00:00'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationTimes_UserMedicines_UserMedicineId",
                table: "MedicationTimes");

            migrationBuilder.DropColumn(
                name: "WeekDay",
                table: "MedicationTimes");

            migrationBuilder.AlterColumn<int>(
                name: "UserMedicineId",
                table: "MedicationTimes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "MedicationTimes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationTimes_UserMedicines_UserMedicineId",
                table: "MedicationTimes",
                column: "UserMedicineId",
                principalTable: "UserMedicines",
                principalColumn: "Id");
            
            migrationBuilder.AddColumn<DateTime>(
                name: "TempTime",
                table: "MedicationTimes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1970, 1, 1));

            // 'Time' sütunundaki değerleri 'TempTime' geçici sütununa aktarın
            // Burada, 'interval' tipinden 'DateTime' tipine dönüşüm yaparken, 
            // varsayılan bir tarih ekleyerek zaman kısmını koruyabiliriz (Örnek: 1970-01-01)
            migrationBuilder.Sql(
                @"
        UPDATE ""MedicationTimes""
        SET ""TempTime"" = '1970-01-01'::date + ""Time""
        ");

            // Orijinal 'Time' sütununu kaldırın
            migrationBuilder.DropColumn(
                name: "Time",
                table: "MedicationTimes");

            // 'TempTime' sütununu 'Time' olarak yeniden adlandırın
            migrationBuilder.RenameColumn(
                name: "TempTime",
                table: "MedicationTimes",
                newName: "Time");
        }
    }
}
