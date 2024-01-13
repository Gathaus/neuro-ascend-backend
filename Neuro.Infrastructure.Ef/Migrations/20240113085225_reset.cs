using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class reset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.CreateTable(
                name: "UserProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LastFoodId = table.Column<int>(type: "integer", nullable: true),
                    LastExerciseId = table.Column<int>(type: "integer", nullable: true),
                    LastActivityId = table.Column<int>(type: "integer", nullable: true),
                    LastArticleId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProgresses_Activities_LastActivityId",
                        column: x => x.LastActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProgresses_Articles_LastArticleId",
                        column: x => x.LastArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProgresses_Exercises_LastExerciseId",
                        column: x => x.LastExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProgresses_FoodPages_LastFoodId",
                        column: x => x.LastFoodId,
                        principalTable: "FoodPages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_LastActivityId",
                table: "UserProgresses",
                column: "LastActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_LastArticleId",
                table: "UserProgresses",
                column: "LastArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_LastExerciseId",
                table: "UserProgresses",
                column: "LastExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_LastFoodId",
                table: "UserProgresses",
                column: "LastFoodId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_UserId",
                table: "UserProgresses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.DropTable(
                name: "UserProgresses");
        }
    }
}
