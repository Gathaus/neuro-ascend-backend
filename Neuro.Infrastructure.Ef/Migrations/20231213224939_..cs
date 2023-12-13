using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "RecommendedArticles",
                table: "Articles",
                type: "integer[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendedArticles",
                table: "Articles");
        }
    }
}
