using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sopheon.CloudNative.Products.DataAccess.Migrations
{
    public partial class TEMP_AddProductMilestone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Tasks",
                schema: "SPM")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "TasksHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "TasksHistory")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", "SPM")
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "SPM",
                table: "Tasks",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Milestones",
                schema: "SPM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Milestones_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                schema: "SPM",
                table: "Attribute",
                keyColumn: "AttributeId",
                keyValue: -4,
                column: "AttributeDataTypeId",
                value: 8);

            migrationBuilder.UpdateData(
                schema: "SPM",
                table: "Attribute",
                keyColumn: "AttributeId",
                keyValue: -1,
                column: "AttributeDataTypeId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "SPM",
                table: "Attribute",
                keyColumn: "AttributeId",
                keyValue: -2,
                column: "AttributeDataTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "SPM",
                table: "Attribute",
                keyColumn: "AttributeId",
                keyValue: -3,
                column: "AttributeDataTypeId",
                value: 5);

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_ProductId",
                schema: "SPM",
                table: "Milestones",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Milestones",
                schema: "SPM");

            migrationBuilder.DeleteData(
                schema: "SPM",
                table: "Attribute",
                keyColumn: "AttributeId",
                keyValue: -4);

            migrationBuilder.DeleteData(
                schema: "SPM",
                table: "Attribute",
                keyColumn: "AttributeId",
                keyValue: -1);

            migrationBuilder.DeleteData(
                schema: "SPM",
                table: "Attribute",
                keyColumn: "AttributeId",
                keyValue: -2);

            migrationBuilder.DeleteData(
                schema: "SPM",
                table: "Attribute",
                keyColumn: "AttributeId",
                keyValue: -3);

            migrationBuilder.AlterTable(
                name: "Tasks",
                schema: "SPM")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "TasksHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "SPM")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "TasksHistory")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "SPM",
                table: "Tasks",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.InsertData(
                schema: "SPM",
                table: "Attribute",
                columns: new[] { "AttributeId", "AttributeDataTypeId", "Name", "ShortName" },
                values: new object[,]
                {
                    { -4, 0, "Status", "STATUS" },
                    { -1, 0, "Industry", "IND" },
                    { -2, 0, "Notes", "NOTES" },
                    { -3, 0, "Due Date", "DUE" }
                });
        }
    }
}
