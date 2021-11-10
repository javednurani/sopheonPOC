namespace Sopheon.CloudNative.Environments.Functions.Models
{
   public class BusinessServiceDependencyDto
   {
      public int Id 
      {
         get;
         set;
      }

      public string DependencyName
      {
         get;
         set;
      }

      public int BusinessServiceId
      {
         get;
         set;
      }

      public int DomainResourceTypeId
      {
         get;
         set;
      }
   }
}
