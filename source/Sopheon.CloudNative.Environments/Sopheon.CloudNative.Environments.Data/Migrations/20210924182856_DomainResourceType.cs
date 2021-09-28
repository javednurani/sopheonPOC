using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class DomainResourceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessServiceDependencies_DomainResourceTypes_ResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_DomainResourceTypes_ResourceTypeId",
                schema: "ENV",
                table: "Resources");

            migrationBuilder.RenameColumn(
                name: "ResourceTypeId",
                schema: "ENV",
                table: "Resources",
                newName: "DomainResourceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Resources_ResourceTypeId",
                schema: "ENV",
                table: "Resources",
                newName: "IX_Resources_DomainResourceTypeId");

            migrationBuilder.RenameColumn(
                name: "ResourceTypeId",
                schema: "ENV",
                table: "DomainResourceTypes",
                newName: "DomainResourceTypeId");

            migrationBuilder.RenameColumn(
                name: "ResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies",
                newName: "DomainResourceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BusinessServiceDependencies_ResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies",
                newName: "IX_BusinessServiceDependencies_DomainResourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessServiceDependencies_DomainResourceTypes_DomainResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies",
                column: "DomainResourceTypeId",
                principalSchema: "ENV",
                principalTable: "DomainResourceTypes",
                principalColumn: "DomainResourceTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_DomainResourceTypes_DomainResourceTypeId",
                schema: "ENV",
                table: "Resources",
                column: "DomainResourceTypeId",
                principalSchema: "ENV",
                principalTable: "DomainResourceTypes",
                principalColumn: "DomainResourceTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessServiceDependencies_DomainResourceTypes_DomainResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Resources_DomainResourceTypes_DomainResourceTypeId",
                schema: "ENV",
                table: "Resources");

            migrationBuilder.RenameColumn(
                name: "DomainResourceTypeId",
                schema: "ENV",
                table: "Resources",
                newName: "ResourceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Resources_DomainResourceTypeId",
                schema: "ENV",
                table: "Resources",
                newName: "IX_Resources_ResourceTypeId");

            migrationBuilder.RenameColumn(
                name: "DomainResourceTypeId",
                schema: "ENV",
                table: "DomainResourceTypes",
                newName: "ResourceTypeId");

            migrationBuilder.RenameColumn(
                name: "DomainResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies",
                newName: "ResourceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BusinessServiceDependencies_DomainResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies",
                newName: "IX_BusinessServiceDependencies_ResourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessServiceDependencies_DomainResourceTypes_ResourceTypeId",
                schema: "ENV",
                table: "BusinessServiceDependencies",
                column: "ResourceTypeId",
                principalSchema: "ENV",
                principalTable: "DomainResourceTypes",
                principalColumn: "ResourceTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_DomainResourceTypes_ResourceTypeId",
                schema: "ENV",
                table: "Resources",
                column: "ResourceTypeId",
                principalSchema: "ENV",
                principalTable: "DomainResourceTypes",
                principalColumn: "ResourceTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
