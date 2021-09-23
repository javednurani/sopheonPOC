using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class AddEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ENV");

            migrationBuilder.RenameTable(
                name: "Environments",
                newName: "Environments",
                newSchema: "ENV");

            migrationBuilder.RenameColumn(
                name: "EnvironmentID",
                schema: "ENV",
                table: "Environments",
                newName: "EnvironmentId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ENV",
                table: "Environments",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "ENV",
                table: "Environments",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

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
                    ResourceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainResourceTypes", x => x.ResourceTypeId);
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
                    ResourceTypeId = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_BusinessServiceDependencies_DomainResourceTypes_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalSchema: "ENV",
                        principalTable: "DomainResourceTypes",
                        principalColumn: "ResourceTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                schema: "ENV",
                columns: table => new
                {
                    ResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceTypeId = table.Column<int>(type: "int", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.ResourceId);
                    table.ForeignKey(
                        name: "FK_Resources_DomainResourceTypes_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalSchema: "ENV",
                        principalTable: "DomainResourceTypes",
                        principalColumn: "ResourceTypeId",
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
                name: "IX_BusinessServiceDependencies_ResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies",
                column: "ResourceTypeId");

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
                name: "IX_Resources_ResourceTypeId",
                schema: "ENV",
                table: "Resources",
                column: "ResourceTypeId");

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
                name: "Resources",
                schema: "ENV");

            migrationBuilder.DropTable(
                name: "BusinessServices",
                schema: "ENV");

            migrationBuilder.DropTable(
                name: "DomainResourceTypes",
                schema: "ENV");

            migrationBuilder.RenameTable(
                name: "Environments",
                schema: "ENV",
                newName: "Environments");

            migrationBuilder.RenameColumn(
                name: "EnvironmentId",
                table: "Environments",
                newName: "EnvironmentID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Environments",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Environments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true);
        }
    }
}
