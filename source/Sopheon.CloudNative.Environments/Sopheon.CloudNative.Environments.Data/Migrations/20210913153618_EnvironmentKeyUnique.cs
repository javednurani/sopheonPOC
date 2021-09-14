using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Domain.Migrations
{
	[ExcludeFromCodeCoverage]
    public partial class EnvironmentKeyUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Environments_EnvironmentKey",
                table: "Environments",
                column: "EnvironmentKey",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Environments_EnvironmentKey",
                table: "Environments");
        }
    }
}
