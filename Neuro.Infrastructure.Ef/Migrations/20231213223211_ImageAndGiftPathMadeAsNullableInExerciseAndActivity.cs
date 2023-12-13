using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class ImageAndGiftPathMadeAsNullableInExerciseAndActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GifPath",
                table: "Exercises",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Activities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GifPath",
                table: "Exercises",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Activities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
