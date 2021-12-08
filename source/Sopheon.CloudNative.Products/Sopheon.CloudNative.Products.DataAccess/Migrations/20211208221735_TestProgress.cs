using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sopheon.CloudNative.Products.DataAccess.Migrations
{
    public partial class TestProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SPM");

            migrationBuilder.CreateTable(
                name: "AttributeDataType",
                schema: "SPM",
                columns: table => new
                {
                    AttributeDataTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeDataType", x => x.AttributeDataTypeId);
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
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                name: "Attribute",
                schema: "SPM",
                columns: table => new
                {
                    AttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeDataTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.AttributeId);
                    table.ForeignKey(
                        name: "FK_Attribute_AttributeDataType_AttributeDataTypeId",
                        column: x => x.AttributeDataTypeId,
                        principalSchema: "SPM",
                        principalTable: "AttributeDataType",
                        principalColumn: "AttributeDataTypeId",
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Goal",
                schema: "SPM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
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
                        principalColumn: "Id");
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
                        principalColumn: "Id");
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
                        principalColumn: "Id");
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductItem_Rank_RankId",
                        column: x => x.RankId,
                        principalSchema: "SPM",
                        principalTable: "Rank",
                        principalColumn: "RankId");
                });

            migrationBuilder.CreateTable(
                name: "EnumAttributeOption",
                schema: "SPM",
                columns: table => new
                {
                    EnumAttributeOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AttributeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumAttributeOption", x => x.EnumAttributeOptionId);
                    table.ForeignKey(
                        name: "FK_EnumAttributeOption_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeyPerformanceIndicator",
                schema: "SPM",
                columns: table => new
                {
                    KeyPerformanceIndicatorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    AttributeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPerformanceIndicator", x => new { x.ProductId, x.KeyPerformanceIndicatorId });
                    table.ForeignKey(
                        name: "FK_KeyPerformanceIndicator_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KeyPerformanceIndicator_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        name: "FK_Products_DecimalAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
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
                name: "Products_EnumCollectionAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductEnumCollectionAttributeValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_EnumCollectionAttributeValues", x => x.ProductEnumCollectionAttributeValueId);
                    table.ForeignKey(
                        name: "FK_Products_EnumCollectionAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_EnumCollectionAttributeValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SPM",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products_Int32AttributeValues",
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
                    table.PrimaryKey("PK_Products_Int32AttributeValues", x => new { x.ProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_Products_Int32AttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Int32AttributeValues_Products_ProductId",
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
                        name: "FK_Products_MoneyAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
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
                        name: "FK_Products_StringAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
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
                        name: "FK_Products_UtcDateTimeAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
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
                        name: "FK_ProductItem_DecimalAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
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
                name: "ProductItem_EnumCollectionAttributeValues",
                schema: "SPM",
                columns: table => new
                {
                    ProductItemEnumCollectionAttributeValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    ProductItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem_EnumCollectionAttributeValues", x => x.ProductItemEnumCollectionAttributeValueId);
                    table.ForeignKey(
                        name: "FK_ProductItem_EnumCollectionAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItem_EnumCollectionAttributeValues_ProductItem_ProductItemId",
                        column: x => x.ProductItemId,
                        principalSchema: "SPM",
                        principalTable: "ProductItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItem_Int32AttributeValues",
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
                    table.PrimaryKey("PK_ProductItem_Int32AttributeValues", x => new { x.ProductItemId, x.Id });
                    table.ForeignKey(
                        name: "FK_ProductItem_Int32AttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItem_Int32AttributeValues_ProductItem_ProductItemId",
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
                        name: "FK_ProductItem_MoneyAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
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
                        name: "FK_ProductItem_StringAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
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
                        name: "FK_ProductItem_UtcDateTimeAttributeValues_Attribute_AttributeId",
                        column: x => x.AttributeId,
                        principalSchema: "SPM",
                        principalTable: "Attribute",
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

            migrationBuilder.CreateTable(
                name: "Products_EnumCollectionAttributeValues_Value",
                schema: "SPM",
                columns: table => new
                {
                    EnumAttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    ProductEnumCollectionAttributeValueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products_EnumCollectionAttributeValues_Value", x => new { x.ProductEnumCollectionAttributeValueId, x.EnumAttributeOptionId });
                    table.ForeignKey(
                        name: "FK_Products_EnumCollectionAttributeValues_Value_EnumAttributeOption_EnumAttributeOptionId",
                        column: x => x.EnumAttributeOptionId,
                        principalSchema: "SPM",
                        principalTable: "EnumAttributeOption",
                        principalColumn: "EnumAttributeOptionId");
                    table.ForeignKey(
                        name: "FK_Products_EnumCollectionAttributeValues_Value_Products_EnumCollectionAttributeValues_ProductEnumCollectionAttributeValueId",
                        column: x => x.ProductEnumCollectionAttributeValueId,
                        principalSchema: "SPM",
                        principalTable: "Products_EnumCollectionAttributeValues",
                        principalColumn: "ProductEnumCollectionAttributeValueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItem_EnumCollectionAttributeValues_Value",
                schema: "SPM",
                columns: table => new
                {
                    EnumAttributeOptionId = table.Column<int>(type: "int", nullable: false),
                    ProductItemEnumCollectionAttributeValueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem_EnumCollectionAttributeValues_Value", x => new { x.ProductItemEnumCollectionAttributeValueId, x.EnumAttributeOptionId });
                    table.ForeignKey(
                        name: "FK_ProductItem_EnumCollectionAttributeValues_Value_EnumAttributeOption_EnumAttributeOptionId",
                        column: x => x.EnumAttributeOptionId,
                        principalSchema: "SPM",
                        principalTable: "EnumAttributeOption",
                        principalColumn: "EnumAttributeOptionId");
                    table.ForeignKey(
                        name: "FK_ProductItem_EnumCollectionAttributeValues_Value_ProductItem_EnumCollectionAttributeValues_ProductItemEnumCollectionAttribute~",
                        column: x => x.ProductItemEnumCollectionAttributeValueId,
                        principalSchema: "SPM",
                        principalTable: "ProductItem_EnumCollectionAttributeValues",
                        principalColumn: "ProductItemEnumCollectionAttributeValueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "SPM",
                table: "AttributeDataType",
                columns: new[] { "AttributeDataTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "String" },
                    { 2, "Int32" },
                    { 3, "Decimal" },
                    { 4, "Money" },
                    { 5, "UtcDateTime" },
                    { 6, "MarkdownString" },
                    { 7, "EnumCollection" }
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
                table: "Attribute",
                columns: new[] { "AttributeId", "AttributeDataTypeId", "Name", "ShortName" },
                values: new object[,]
                {
                    { -4, 7, "Status", null },
                    { -1, 2, "Industry", "IND" },
                    { -2, 1, "Notes", "NOTES" },
                    { -3, 5, "Due Date", "DUE" }
                });

            migrationBuilder.InsertData(
                schema: "SPM",
                table: "EnumAttributeOption",
                columns: new[] { "EnumAttributeOptionId", "AttributeId", "Name" },
                values: new object[,]
                {
                    { -4, -4, "Complete" },
                    { -3, -4, "Assigned" },
                    { -2, -4, "In Progress" },
                    { -1, -4, "Not Started" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_AttributeDataTypeId",
                schema: "SPM",
                table: "Attribute",
                column: "AttributeDataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumAttributeOption_AttributeId_Name",
                schema: "SPM",
                table: "EnumAttributeOption",
                columns: new[] { "AttributeId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

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
                name: "IX_ProductItem_EnumCollectionAttributeValues_AttributeId",
                schema: "SPM",
                table: "ProductItem_EnumCollectionAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_EnumCollectionAttributeValues_ProductItemId",
                schema: "SPM",
                table: "ProductItem_EnumCollectionAttributeValues",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_EnumCollectionAttributeValues_Value_EnumAttributeOptionId",
                schema: "SPM",
                table: "ProductItem_EnumCollectionAttributeValues_Value",
                column: "EnumAttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_Int32AttributeValues_AttributeId",
                schema: "SPM",
                table: "ProductItem_Int32AttributeValues",
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
                name: "IX_Products_Key",
                schema: "SPM",
                table: "Products",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DecimalAttributeValues_AttributeId",
                schema: "SPM",
                table: "Products_DecimalAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_EnumCollectionAttributeValues_AttributeId",
                schema: "SPM",
                table: "Products_EnumCollectionAttributeValues",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_EnumCollectionAttributeValues_ProductId",
                schema: "SPM",
                table: "Products_EnumCollectionAttributeValues",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_EnumCollectionAttributeValues_Value_EnumAttributeOptionId",
                schema: "SPM",
                table: "Products_EnumCollectionAttributeValues_Value",
                column: "EnumAttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Int32AttributeValues_AttributeId",
                schema: "SPM",
                table: "Products_Int32AttributeValues",
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
                name: "ProductItem_EnumCollectionAttributeValues_Value",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "ProductItem_Int32AttributeValues",
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
                name: "Products_EnumCollectionAttributeValues_Value",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Products_Int32AttributeValues",
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
                name: "ProductItem_EnumCollectionAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "EnumAttributeOption",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Products_EnumCollectionAttributeValues",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "ProductItem",
                schema: "SPM");

            migrationBuilder.DropTable(
                name: "Attribute",
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
                name: "AttributeDataType",
                schema: "SPM");
        }
    }
}
