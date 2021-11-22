using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Products.DataAccess.Migrations
{
    public partial class kpi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyPerformanceIndicator",
                schema: "SPM",
                columns: table => new
                {
                    KeyPerformanceIndicatorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPerformanceIndicator", x => x.KeyPerformanceIndicatorId);
                    table.ForeignKey(
                        name: "FK_KeyPerformanceIndicator_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KeyPerformanceIndicator_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyPerformanceIndicator_AttributeId",
                schema: "SPM",
                table: "KeyPerformanceIndicator",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyPerformanceIndicator_ProductId",
                schema: "SPM",
                table: "KeyPerformanceIndicator",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyPerformanceIndicator",
                schema: "SPM");
        }
    }
}
