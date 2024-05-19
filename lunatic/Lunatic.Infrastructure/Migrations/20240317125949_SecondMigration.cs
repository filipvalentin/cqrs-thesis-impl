using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lunatic.Infrastructure.Migrations {
	/// <inheritdoc />
	public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Images",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Images",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "PublicId");
        }
    }
}
