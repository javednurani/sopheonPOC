using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class BusinessService_Add_ProductManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ENV",
                table: "BusinessServices",
                columns: new[] { "BusinessServiceId", "Name" },
                values: new object[] { 1, "ProductManagement" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ENV",
                table: "BusinessServices",
                keyColumn: "BusinessServiceId",
                keyValue: 1);
        }
    }
}
