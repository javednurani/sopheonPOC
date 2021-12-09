using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class AddSqlServerResourceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ENV",
                table: "BusinessServices",
                columns: new[] { "BusinessServiceId", "Name" },
                values: new object[] { 1, "ProductManagement" });

            migrationBuilder.InsertData(
                schema: "ENV",
                table: "DomainResourceTypes",
                columns: new[] { "DomainResourceTypeId", "IsDedicated", "Name" },
                values: new object[] { 3, false, "TenantAzureSqlServer" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "BusinessServices",
                keyColumn: "BusinessServiceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "DomainResourceTypes",
                keyColumn: "DomainResourceTypeId",
                keyValue: 3);
        }
    }
}
