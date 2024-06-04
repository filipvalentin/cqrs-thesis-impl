using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lunatic.Infrastructure.Migrations.LunaticML
{
    /// <inheritdoc />
    public partial class MLMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysTookToComplete",
                table: "DaysToCompleteTaskEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedDaysToComplete",
                table: "DaysToCompleteTaskEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysTookToComplete",
                table: "DaysToCompleteTaskEntries");

            migrationBuilder.DropColumn(
                name: "ExpectedDaysToComplete",
                table: "DaysToCompleteTaskEntries");
        }
    }
}
