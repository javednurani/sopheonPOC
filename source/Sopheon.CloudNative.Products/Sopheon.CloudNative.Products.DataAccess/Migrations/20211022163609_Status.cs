using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Products.DataAccess.Migrations
{
   public partial class Status : Migration
   {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.UpdateData(
             schema: "SPM",
             table: "AttributeValueType",
             keyColumn: "AttributeValueTypeId",
             keyValue: 2,
             column: "Name",
             value: "Int32");

         migrationBuilder.UpdateData(
             schema: "SPM",
             table: "AttributeValueType",
             keyColumn: "AttributeValueTypeId",
             keyValue: 4,
             column: "Name",
             value: "Money");

         migrationBuilder.InsertData(
             schema: "SPM",
             table: "AttributeValueType",
             columns: new[] { "AttributeValueTypeId", "Name" },
             values: new object[] { 6, "MarkdownString" });
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DeleteData(
             schema: "SPM",
             table: "AttributeValueType",
             keyColumn: "AttributeValueTypeId",
             keyValue: 6);

         migrationBuilder.UpdateData(
             schema: "SPM",
             table: "AttributeValueType",
             keyColumn: "AttributeValueTypeId",
             keyValue: 2,
             column: "Name",
             value: "Int");

         migrationBuilder.UpdateData(
             schema: "SPM",
             table: "AttributeValueType",
             keyColumn: "AttributeValueTypeId",
             keyValue: 4,
             column: "Name",
             value: "Currency");
      }
   }
}
