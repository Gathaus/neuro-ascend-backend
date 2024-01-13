using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate12312333313121 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "AlzheimerLevel",
                table: "Activities",
                type: "smallint[]",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "smallint[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "AlzheimerLevel",
                table: "Activities",
                type: "smallint[]",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "smallint[]",
                oldNullable: true);
        }
    }
}
