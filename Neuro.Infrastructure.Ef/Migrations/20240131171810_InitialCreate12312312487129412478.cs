using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate12312312487129412478 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
        ALTER TABLE ""TimesOfDay""
        ALTER COLUMN ""Time"" TYPE timestamp with time zone
        USING (CURRENT_DATE + ""Time"");
        "
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
        ALTER TABLE ""TimesOfDay""
        ALTER COLUMN ""Time"" TYPE interval
        USING '1 day' * EXTRACT(EPOCH FROM ""Time"") / 86400;
        "
            );
        }
    }
}
