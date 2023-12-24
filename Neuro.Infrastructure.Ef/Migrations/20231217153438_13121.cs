using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neuro.Infrastructure.Ef.Migrations
{
    /// <inheritdoc />
    public partial class _13121 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TempImageName",
                table: "Users",
                newName: "Usage");

            migrationBuilder.RenameColumn(
                name: "ReminderTimeStr",
                table: "Users",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "MobileNumber");

            migrationBuilder.RenameColumn(
                name: "MedicationDays",
                table: "Users",
                newName: "SelectedDays");

            migrationBuilder.RenameColumn(
                name: "HowToUse",
                table: "Users",
                newName: "EndMonth");

            migrationBuilder.RenameColumn(
                name: "Disease",
                table: "Users",
                newName: "DiseaseTerm");

            migrationBuilder.RenameColumn(
                name: "AlzheimerStage",
                table: "Users",
                newName: "DiseaseLevel");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Amount",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BeginDay",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BeginMonth",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryCallingCode",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EndDay",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BeginDay",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BeginMonth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountryCallingCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EndDay",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Usage",
                table: "Users",
                newName: "TempImageName");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Users",
                newName: "ReminderTimeStr");

            migrationBuilder.RenameColumn(
                name: "SelectedDays",
                table: "Users",
                newName: "MedicationDays");

            migrationBuilder.RenameColumn(
                name: "MobileNumber",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "EndMonth",
                table: "Users",
                newName: "HowToUse");

            migrationBuilder.RenameColumn(
                name: "DiseaseTerm",
                table: "Users",
                newName: "Disease");

            migrationBuilder.RenameColumn(
                name: "DiseaseLevel",
                table: "Users",
                newName: "AlzheimerStage");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
