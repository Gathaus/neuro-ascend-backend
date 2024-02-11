using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class f4312318 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diseases_Users_UserId",
                table: "Diseases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMedicines_Users_UserId",
                table: "UserMedicines");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserMedicines",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Diseases_Users_UserId",
                table: "Diseases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMedicines_Users_UserId",
                table: "UserMedicines",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diseases_Users_UserId",
                table: "Diseases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMedicines_Users_UserId",
                table: "UserMedicines");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserMedicines",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Diseases_Users_UserId",
                table: "Diseases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMedicines_Users_UserId",
                table: "UserMedicines",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
