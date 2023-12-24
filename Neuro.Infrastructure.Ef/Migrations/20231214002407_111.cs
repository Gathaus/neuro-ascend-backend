using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "RecommendedRoutines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "RecommendedRoutines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "RecommendedRoutines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoodId",
                table: "RecommendedRoutines",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRoutines_ActivityId",
                table: "RecommendedRoutines",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRoutines_ArticleId",
                table: "RecommendedRoutines",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRoutines_ExerciseId",
                table: "RecommendedRoutines",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedRoutines_FoodId",
                table: "RecommendedRoutines",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedRoutines_Activities_ActivityId",
                table: "RecommendedRoutines",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedRoutines_Articles_ArticleId",
                table: "RecommendedRoutines",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedRoutines_Exercises_ExerciseId",
                table: "RecommendedRoutines",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendedRoutines_FoodPages_FoodId",
                table: "RecommendedRoutines",
                column: "FoodId",
                principalTable: "FoodPages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedRoutines_Activities_ActivityId",
                table: "RecommendedRoutines");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedRoutines_Articles_ArticleId",
                table: "RecommendedRoutines");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedRoutines_Exercises_ExerciseId",
                table: "RecommendedRoutines");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendedRoutines_FoodPages_FoodId",
                table: "RecommendedRoutines");

            migrationBuilder.DropIndex(
                name: "IX_RecommendedRoutines_ActivityId",
                table: "RecommendedRoutines");

            migrationBuilder.DropIndex(
                name: "IX_RecommendedRoutines_ArticleId",
                table: "RecommendedRoutines");

            migrationBuilder.DropIndex(
                name: "IX_RecommendedRoutines_ExerciseId",
                table: "RecommendedRoutines");

            migrationBuilder.DropIndex(
                name: "IX_RecommendedRoutines_FoodId",
                table: "RecommendedRoutines");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "RecommendedRoutines");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "RecommendedRoutines");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "RecommendedRoutines");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "RecommendedRoutines");
        }
    }
}
