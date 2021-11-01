using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopheon.CloudNative.Environments.Data.Migrations
{
   public partial class BusinessService_CustomSql_DeleteAll : Migration
   {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         // Delete all BusinessServices and related records
         // Next Migration, BusinessService_Add_ProductManagement, will populate ENV.BusinessServices table from the Enum Sopheon.CloudNative.Environments.Domain.Enums.BusinessServices
         migrationBuilder.Sql("DELETE ERB FROM [ENV].[ENVIRONMENTRESOURCEBINDINGS] ERB");
         migrationBuilder.Sql("DELETE BSD FROM [ENV].[BUSINESSSERVICEDEPENDENCIES] BSD");
         migrationBuilder.Sql("DELETE BS FROM [ENV].[BUSINESSSERVICES] BS");
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {

      }
   }
}
