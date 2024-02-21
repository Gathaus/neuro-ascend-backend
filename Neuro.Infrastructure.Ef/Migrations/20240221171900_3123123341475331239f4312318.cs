using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _3123123341475331239f4312318 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MedicineTarget = table.Column<short>(type: "smallint", nullable: true),
                    MorningFoodTarget = table.Column<short>(type: "smallint", nullable: true),
                    EveningFoodTarget = table.Column<short>(type: "smallint", nullable: true),
                    ActivityTarget = table.Column<short>(type: "smallint", nullable: true),
                    ExerciseTarget = table.Column<short>(type: "smallint", nullable: true),
                    ArticleTarget = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TargetGroupId = table.Column<int>(type: "integer", nullable: false),
                    MedicineTaken = table.Column<short>(type: "smallint", nullable: true),
                    MorningFoodTaken = table.Column<short>(type: "smallint", nullable: true),
                    EveningFoodTaken = table.Column<short>(type: "smallint", nullable: true),
                    ActivityDone = table.Column<short>(type: "smallint", nullable: true),
                    ExerciseDone = table.Column<short>(type: "smallint", nullable: true),
                    ArticleDone = table.Column<short>(type: "smallint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTargets_TargetGroups_TargetGroupId",
                        column: x => x.TargetGroupId,
                        principalTable: "TargetGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTargets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTargets_TargetGroupId",
                table: "UserTargets",
                column: "TargetGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTargets_UserId",
                table: "UserTargets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTargets");

            migrationBuilder.DropTable(
                name: "TargetGroups");
        }
    }
}
