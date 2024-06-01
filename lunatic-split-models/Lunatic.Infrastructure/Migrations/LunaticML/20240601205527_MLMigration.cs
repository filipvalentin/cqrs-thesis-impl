using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lunatic.Infrastructure.Migrations.LunaticML
{
    /// <inheritdoc />
    public partial class MLMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DaysToCompleteTaskEntries",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssigneesCount = table.Column<int>(type: "int", nullable: false),
                    DescriptionLength = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false),
                    AverageCommentLength = table.Column<double>(type: "float", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysToCompleteTaskEntries", x => x.TaskId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaysToCompleteTaskEntries");
        }
    }
}
