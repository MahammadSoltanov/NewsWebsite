using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteUsellessProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PostTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PostTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PostTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PostTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PostHashtags");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PostHashtags");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PostHashtags");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PostHashtags");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Hashtags");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Hashtags");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Hashtags");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Hashtags");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CategoryTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CategoryTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CategoryTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CategoryTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "PostTranslations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PostTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "PostTranslations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "PostTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "PostHashtags",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PostHashtags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "PostHashtags",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "PostHashtags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Languages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Languages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Languages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Languages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Hashtags",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Hashtags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Hashtags",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Hashtags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "CategoryTranslations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CategoryTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "CategoryTranslations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CategoryTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Categories",
                type: "datetime2",
                nullable: true);
        }
    }
}
