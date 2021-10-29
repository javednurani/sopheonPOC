using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class DomainResourceType_Seed_IsDedicated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "ENV",
                table: "DomainResourceTypes",
                keyColumn: "DomainResourceTypeId",
                keyValue: 1,
                column: "IsDedicated",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "ENV",
                table: "DomainResourceTypes",
                keyColumn: "DomainResourceTypeId",
                keyValue: 1,
                column: "IsDedicated",
                value: false);
        }
    }
}
