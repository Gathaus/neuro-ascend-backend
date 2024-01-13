using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate12312333313122121312 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_FoodPages_LastFoodId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_LastFoodId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "LastFoodId",
                table: "UserProgresses");

            migrationBuilder.AddColumn<int>(
                name: "EveningLastFoodId",
                table: "UserProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MorningLastFoodId",
                table: "UserProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_EveningLastFoodId",
                table: "UserProgresses",
                column: "EveningLastFoodId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_MorningLastFoodId",
                table: "UserProgresses",
                column: "MorningLastFoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_FoodPages_EveningLastFoodId",
                table: "UserProgresses",
                column: "EveningLastFoodId",
                principalTable: "FoodPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_FoodPages_MorningLastFoodId",
                table: "UserProgresses",
                column: "MorningLastFoodId",
                principalTable: "FoodPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_FoodPages_EveningLastFoodId",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_FoodPages_MorningLastFoodId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_EveningLastFoodId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_MorningLastFoodId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "EveningLastFoodId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "MorningLastFoodId",
                table: "UserProgresses");

            migrationBuilder.AddColumn<int>(
                name: "LastFoodId",
                table: "UserProgresses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_LastFoodId",
                table: "UserProgresses",
                column: "LastFoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_FoodPages_LastFoodId",
                table: "UserProgresses",
                column: "LastFoodId",
                principalTable: "FoodPages",
                principalColumn: "Id");
        }
    }
}
