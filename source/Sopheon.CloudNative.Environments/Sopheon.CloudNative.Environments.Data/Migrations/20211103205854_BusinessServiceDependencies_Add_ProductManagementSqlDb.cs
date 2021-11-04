using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class BusinessServiceDependencies_Add_ProductManagementSqlDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ENV",
                table: "BusinessServiceDependencies",
                columns: new[] { "BusinessServiceDependencyId", "BusinessServiceId", "DependencyName", "DomainResourceTypeId" },
                values: new object[] { 1, 1, "ProductManagementSqlDb", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "BusinessServiceDependencies",
                keyColumn: "BusinessServiceDependencyId",
                keyValue: 1);
        }
    }
}
