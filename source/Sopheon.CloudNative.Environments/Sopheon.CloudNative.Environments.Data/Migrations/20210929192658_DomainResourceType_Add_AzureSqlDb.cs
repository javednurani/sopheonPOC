using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class DomainResourceType_Add_AzureSqlDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ENV",
                table: "DomainResourceTypes",
                columns: new[] { "DomainResourceTypeId", "Name" },
                values: new object[] { 1, "AzureSqlDb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "DomainResourceTypes",
                keyColumn: "DomainResourceTypeId",
                keyValue: 1);
        }
    }
}
