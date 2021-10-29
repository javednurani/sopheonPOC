using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class DedicatedEnvironmentResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDedicated",
                schema: "ENV",
                table: "DomainResourceTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DedicatedEnvironmentResources",
                schema: "ENV",
                columns: table => new
                {
                    DedicatedEnvironmentResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnvironmentId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DedicatedEnvironmentResources", x => x.DedicatedEnvironmentResourceId);
                    table.ForeignKey(
                        name: "FK_DedicatedEnvironmentResources_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalSchema: "ENV",
                        principalTable: "Environments",
                        principalColumn: "EnvironmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DedicatedEnvironmentResources_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "ENV",
                        principalTable: "Resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DedicatedEnvironmentResources_EnvironmentId",
                schema: "ENV",
                table: "DedicatedEnvironmentResources",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DedicatedEnvironmentResources_ResourceId",
                schema: "ENV",
                table: "DedicatedEnvironmentResources",
                column: "ResourceId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DedicatedEnvironmentResources",
                schema: "ENV");

            migrationBuilder.DropColumn(
                name: "IsDedicated",
                schema: "ENV",
                table: "DomainResourceTypes");
        }
    }
}
