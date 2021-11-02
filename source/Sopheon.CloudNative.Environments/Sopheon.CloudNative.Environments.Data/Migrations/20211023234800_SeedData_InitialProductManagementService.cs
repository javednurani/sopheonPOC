using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class SeedData_InitialProductManagementService : Migration
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
                columns: new[] { "DomainResourceTypeId", "Name" },
                values: new object[] { 1, "SqlDatabase" });

            migrationBuilder.InsertData(
                schema: "ENV",
                table: "DomainResourceTypes",
                columns: new[] { "DomainResourceTypeId", "Name" },
                values: new object[] { 2, "AzureBlobStorageContainer" });

            migrationBuilder.InsertData(
                schema: "ENV",
                table: "BusinessServiceDependencies",
                columns: new[] { "BusinessServiceDependencyId", "BusinessServiceId", "DependencyName", "DomainResourceTypeId" },
                values: new object[] { 1, 1, "SqlDatabase", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "BusinessServiceDependencies",
                keyColumn: "BusinessServiceDependencyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "DomainResourceTypes",
                keyColumn: "DomainResourceTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "BusinessServices",
                keyColumn: "BusinessServiceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "DomainResourceTypes",
                keyColumn: "DomainResourceTypeId",
                keyValue: 1);
        }
    }
}
