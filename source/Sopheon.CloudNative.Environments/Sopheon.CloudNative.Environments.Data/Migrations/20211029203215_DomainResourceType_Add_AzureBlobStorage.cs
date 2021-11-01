using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class DomainResourceType_Add_AzureBlobStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ENV",
                table: "DomainResourceTypes",
                columns: new[] { "DomainResourceTypeId", "IsDedicated", "Name" },
                values: new object[] { 2, false, "AzureBlobStorage" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "DomainResourceTypes",
                keyColumn: "DomainResourceTypeId",
                keyValue: 2);
        }
    }
}
