using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class ImageAndGift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "RecommendedRecipes",
                table: "FoodPages",
                type: "integer[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendedRecipes",
                table: "FoodPages");
        }
    }
}
