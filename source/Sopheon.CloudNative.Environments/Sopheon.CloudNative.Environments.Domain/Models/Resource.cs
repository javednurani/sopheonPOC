namespace Sopheon.CloudNative.Environments.Domain.Models
{
   public class Resource
   {
      public int ResourceId
      {
         get;
         set;
      }

      public int ResourceTypeId
      {
         get;
         set;
      }

      public virtual ResourceType ResourceType
      {
         get;
         set;
      }

      public string Name
      {
         get;
         set;
      }
      public string Uri
      {
         get;
         set;
      }
   }
}
