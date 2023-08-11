using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageUrls",
                table: "ImageUrls");

            migrationBuilder.RenameTable(
                name: "ImageUrls",
                newName: "Images");

            migrationBuilder.RenameIndex(
                name: "IX_ImageUrls_Url",
                table: "Images",
                newName: "IX_Images_Url");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "ImageUrls");

            migrationBuilder.RenameIndex(
                name: "IX_Images_Url",
                table: "ImageUrls",
                newName: "IX_ImageUrls_Url");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageUrls",
                table: "ImageUrls",
                column: "Id");
        }
    }
}
