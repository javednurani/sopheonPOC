using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
    public partial class BusinessServiceDependencies_CustomSql_DeleteAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         // Delete all BusinessServiceDependencies and related records
         // Next Migration, BusinessServiceDependencies_Add_ProductManagementSqlDb, will populate ENV.BusinessServiceDependencies table from the Enum Sopheon.CloudNative.Environments.Domain.Enums.BusinessServiceDepdendencies
         migrationBuilder.Sql("DELETE ERB FROM [ENV].[ENVIRONMENTRESOURCEBINDINGS] ERB");
         migrationBuilder.Sql("DELETE BSD FROM [ENV].[BUSINESSSERVICEDEPENDENCIES] BSD");
      }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
