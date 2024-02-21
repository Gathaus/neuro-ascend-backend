using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _53123123341475331239f4312318 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicineTarget",
                table: "TargetGroups");

            migrationBuilder.AddColumn<short>(
                name: "MedicineTarget",
                table: "UserTargets",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicineTarget",
                table: "UserTargets");

            migrationBuilder.AddColumn<short>(
                name: "MedicineTarget",
                table: "TargetGroups",
                type: "smallint",
                nullable: true);
        }
    }
}
