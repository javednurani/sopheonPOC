using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ENV");

            migrationBuilder.CreateTable(
                name: "BusinessServices",
                schema: "ENV",
                columns: table => new
                {
                    BusinessServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessServices", x => x.BusinessServiceId);
                });

            migrationBuilder.CreateTable(
                name: "DomainResourceTypes",
                schema: "ENV",
                columns: table => new
                {
                    DomainResourceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainResourceTypes", x => x.DomainResourceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Environments",
                schema: "ENV",
                columns: table => new
                {
                    EnvironmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnvironmentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.EnvironmentId);
                });

            migrationBuilder.CreateTable(
                name: "BusinessServiceDependencies",
                schema: "ENV",
                columns: table => new
                {
                    BusinessServiceDependencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DependencyName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    BusinessServiceId = table.Column<int>(type: "int", nullable: false),
                    DomainResourceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessServiceDependencies", x => x.BusinessServiceDependencyId);
                    table.ForeignKey(
                        name: "FK_BusinessServiceDependencies_BusinessServices_BusinessServiceId",
                        column: x => x.BusinessServiceId,
                        principalSchema: "ENV",
                        principalTable: "BusinessServices",
                        principalColumn: "BusinessServiceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessServiceDependencies_DomainResourceTypes_DomainResourceTypeId",
                        column: x => x.DomainResourceTypeId,
                        principalSchema: "ENV",
                        principalTable: "DomainResourceTypes",
                        principalColumn: "DomainResourceTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                schema: "ENV",
                columns: table => new
                {
                    ResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainResourceTypeId = table.Column<int>(type: "int", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.ResourceId);
                    table.ForeignKey(
                        name: "FK_Resources_DomainResourceTypes_DomainResourceTypeId",
                        column: x => x.DomainResourceTypeId,
                        principalSchema: "ENV",
                        principalTable: "DomainResourceTypes",
                        principalColumn: "DomainResourceTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentResourceBindings",
                schema: "ENV",
                columns: table => new
                {
                    EnvironmentResourceBindingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnvironmentId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    BusinessServiceDependencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentResourceBindings", x => x.EnvironmentResourceBindingId);
                    table.ForeignKey(
                        name: "FK_EnvironmentResourceBindings_BusinessServiceDependencies_BusinessServiceDependencyId",
                        column: x => x.BusinessServiceDependencyId,
                        principalSchema: "ENV",
                        principalTable: "BusinessServiceDependencies",
                        principalColumn: "BusinessServiceDependencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnvironmentResourceBindings_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalSchema: "ENV",
                        principalTable: "Environments",
                        principalColumn: "EnvironmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnvironmentResourceBindings_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "ENV",
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessServiceDependencies_BusinessServiceId_DependencyName",
                schema: "ENV",
                table: "BusinessServiceDependencies",
                columns: new[] { "BusinessServiceId", "DependencyName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessServiceDependencies_DomainResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies",
                column: "DomainResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessServices_Name",
                schema: "ENV",
                table: "BusinessServices",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentResourceBindings_BusinessServiceDependencyId",
                schema: "ENV",
                table: "EnvironmentResourceBindings",
                column: "BusinessServiceDependencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentResourceBindings_EnvironmentId_BusinessServiceDependencyId",
                schema: "ENV",
                table: "EnvironmentResourceBindings",
                columns: new[] { "EnvironmentId", "BusinessServiceDependencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentResourceBindings_ResourceId",
                schema: "ENV",
                table: "EnvironmentResourceBindings",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_EnvironmentKey",
                schema: "ENV",
                table: "Environments",
                column: "EnvironmentKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_DomainResourceTypeId",
                schema: "ENV",
                table: "Resources",
                column: "DomainResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_Uri",
                schema: "ENV",
                table: "Resources",
                column: "Uri",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnvironmentResourceBindings",
                schema: "ENV");

            migrationBuilder.DropTable(
                name: "BusinessServiceDependencies",
                schema: "ENV");

            migrationBuilder.DropTable(
                name: "Environments",
                schema: "ENV");

            migrationBuilder.DropTable(
                name: "Resources",
                schema: "ENV");

            migrationBuilder.DropTable(
                name: "BusinessServices",
                schema: "ENV");

            migrationBuilder.DropTable(
                name: "DomainResourceTypes",
                schema: "ENV");
        }
    }
}
