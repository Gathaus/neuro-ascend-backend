using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    public partial class AlzheimerLevelTurnedAsListInActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add a SQL statement to manually cast and alter the column
            migrationBuilder.Sql(
                "ALTER TABLE \"Activities\" " +
                "ALTER COLUMN \"AlzheimerLevel\" TYPE smallint[] " +
                "USING ARRAY[\"AlzheimerLevel\"]::smallint[];"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert back to the original type
            migrationBuilder.Sql(
                "ALTER TABLE \"Activities\" " +
                "ALTER COLUMN \"AlzheimerLevel\" TYPE smallint " +
                "USING \"AlzheimerLevel\"[1];" // Assuming the original value is at the first index of the array
            );
        }
    }
}