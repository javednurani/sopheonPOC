using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Domain.Migrations
{
    public partial class AddEnvironmentProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Environments",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Environments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EnvironmentKey",
                table: "Environments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Environments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "Owner",
                table: "Environments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "EnvironmentKey",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Environments");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Environments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}
