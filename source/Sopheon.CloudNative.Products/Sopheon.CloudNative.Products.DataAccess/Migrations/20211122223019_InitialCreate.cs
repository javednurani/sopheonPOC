using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Products.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SPM");

            migrationBuilder.CreateTable(
                name: "AttributeValueType",
                schema: "SPM",
                columns: table => new
                {
                    AttributeValueTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeValueType", x => x.AttributeValueTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProductItemType",
                schema: "SPM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItemType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "SPM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rank",
                schema: "SPM",
                columns: table => new
                {
                    RankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rank", x => x.RankId);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                schema: "SPM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                schema: "SPM",
                columns: table => new
                {
                    AttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeValueTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.AttributeId);
                    table.ForeignKey(
                        name: "FK_Attributes_AttributeValueType_AttributeValueTypeId",
                        column: x => x.AttributeValueTypeId,
                        principalSchema: "SPM",
                        principalTable: "AttributeValueType",
                        principalColumn: "AttributeValueTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileAttachment",
                schema: "SPM",
                columns: table => new
                {
                    FileAttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachment", x => x.FileAttachmentId);
                    table.ForeignKey(
                        name: "FK_FileAttachment_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Goal",
                schema: "SPM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goal_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Release",
                schema: "SPM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Release", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Release_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UrlLink",
                schema: "SPM",
                columns: table => new
                {
                    UrlLinkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlLink", x => x.UrlLinkId);
                    table.ForeignKey(
                        name: "FK_UrlLink_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductItem",
                schema: "SPM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductItemTypeId = table.Column<int>(type: "int", nullable: false),
                    RankId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductItem_ProductItemType_ProductItemTypeId",
                        column: x => x.ProductItemTypeId,
                        principalSchema: "SPM",
                        principalTable: "ProductItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductItem_Rank_RankId",
                        column: x => x.RankId,
                        principalSchema: "SPM",
                        principalTable: "Rank",
                        principalColumn: "RankId",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "Products_DecimalAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_DecimalAttributeValues", x => new { x.ProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_Products_DecimalAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_DecimalAttributeValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products_IntAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_IntAttributeValues", x => new { x.ProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_Products_IntAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_IntAttributeValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products_MoneyAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_MoneyAttributeValues", x => new { x.ProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_Products_MoneyAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_MoneyAttributeValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products_StringAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_StringAttributeValues", x => new { x.ProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_Products_StringAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_StringAttributeValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products_UtcDateTimeAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_UtcDateTimeAttributeValues", x => new { x.ProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_Products_UtcDateTimeAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_UtcDateTimeAttributeValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItem_DecimalAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductItemId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem_DecimalAttributeValues", x => new { x.ProductItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProductItem_DecimalAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItem_DecimalAttributeValues_ProductItem_ProductItemId",
                        column: x => x.ProductItemId,
                        principalSchema: "SPM",
                        principalTable: "ProductItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItem_IntAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductItemId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem_IntAttributeValues", x => new { x.ProductItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProductItem_IntAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItem_IntAttributeValues_ProductItem_ProductItemId",
                        column: x => x.ProductItemId,
                        principalSchema: "SPM",
                        principalTable: "ProductItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItem_MoneyAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductItemId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem_MoneyAttributeValues", x => new { x.ProductItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProductItem_MoneyAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItem_MoneyAttributeValues_ProductItem_ProductItemId",
                        column: x => x.ProductItemId,
                        principalSchema: "SPM",
                        principalTable: "ProductItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItem_StringAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductItemId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem_StringAttributeValues", x => new { x.ProductItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProductItem_StringAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItem_StringAttributeValues_ProductItem_ProductItemId",
                        column: x => x.ProductItemId,
                        principalSchema: "SPM",
                        principalTable: "ProductItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItem_UtcDateTimeAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductItemId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem_UtcDateTimeAttributeValues", x => new { x.ProductItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProductItem_UtcDateTimeAttributeValues_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItem_UtcDateTimeAttributeValues_ProductItem_ProductItemId",
                        column: x => x.ProductItemId,
                        principalSchema: "SPM",
                        principalTable: "ProductItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "SPM",
                table: "AttributeValueType",
                columns: new[] { "AttributeValueTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "String" },
                    { 2, "Int32" },
                    { 3, "Decimal" },
                    { 4, "Money" },
                    { 5, "UtcDateTime" },
                    { 6, "MarkdownString" }
                });

            migrationBuilder.InsertData(
                schema: "SPM",
                table: "ProductItemType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -3, "Risk" },
                    { -2, "Feature" },
                    { -1, "Task" }
                });

            migrationBuilder.InsertData(
                schema: "SPM",
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -2, "Closed" },
                    { -1, "Open" }
                });

            migrationBuilder.InsertData(
                schema: "SPM",
                table: "Attributes",
                columns: new[] { "AttributeId", "AttributeValueTypeId", "Name", "ShortName" },
                values: new object[] { -1, 2, "Industry", null });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_AttributeValueTypeId",
                schema: "SPM",
                table: "Attributes",
                column: "AttributeValueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachment_ProductId",
                schema: "SPM",
                table: "FileAttachment",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Goal_ProductId",
                schema: "SPM",
                table: "Goal",
                column: "ProductId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_ProductId",
                schema: "SPM",
                table: "ProductItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_ProductItemTypeId",
                schema: "SPM",
                table: "ProductItem",
                column: "ProductItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_RankId",
                schema: "SPM",
                table: "ProductItem",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_DecimalAttributeValues_AttributeId",
                schema: "SPM",
                table: "ProductItem_DecimalAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_IntAttributeValues_AttributeId",
                schema: "SPM",
                table: "ProductItem_IntAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_MoneyAttributeValues_AttributeId",
                schema: "SPM",
                table: "ProductItem_MoneyAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_StringAttributeValues_AttributeId",
                schema: "SPM",
                table: "ProductItem_StringAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_UtcDateTimeAttributeValues_AttributeId",
                schema: "SPM",
                table: "ProductItem_UtcDateTimeAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DecimalAttributeValues_AttributeId",
                schema: "SPM",
                table: "Products_DecimalAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IntAttributeValues_AttributeId",
                schema: "SPM",
                table: "Products_IntAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MoneyAttributeValues_AttributeId",
                schema: "SPM",
                table: "Products_MoneyAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StringAttributeValues_AttributeId",
                schema: "SPM",
                table: "Products_StringAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UtcDateTimeAttributeValues_AttributeId",
                schema: "SPM",
                table: "Products_UtcDateTimeAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Release_ProductId",
                schema: "SPM",
                table: "Release",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UrlLink_ProductId",
                schema: "SPM",
                table: "UrlLink",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileAttachment",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Goal",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "KeyPerformanceIndicator",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "ProductItem_DecimalAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "ProductItem_IntAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "ProductItem_MoneyAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "ProductItem_StringAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "ProductItem_UtcDateTimeAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Products_DecimalAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Products_IntAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Products_MoneyAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Products_StringAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Products_UtcDateTimeAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Release",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Status",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "UrlLink",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "ProductItem",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Attributes",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "ProductItemType",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Rank",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "AttributeValueType",
                schema: "SPM");
        }
    }
}
