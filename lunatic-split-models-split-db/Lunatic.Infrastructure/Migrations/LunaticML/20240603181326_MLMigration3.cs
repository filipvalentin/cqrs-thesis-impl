using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lunatic.Infrastructure.Migrations.LunaticML
{
    /// <inheritdoc />
    public partial class MLMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DaysToCompleteTaskEntries",
                table: "DaysToCompleteTaskEntries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_DaysToCompleteTaskEntries",
                table: "DaysToCompleteTaskEntries",
                column: "TaskId");
        }
    }
}
