namespace Sopheon.CloudNative.Products.Domain
{
   /// <summary>
   /// Describes a delta comparison to be performed against an Entity instance and another Target
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class DeltaPair<T>
   {
      public T Entity { get; set; }
      public T CompareTarget { get; set; }
   }
}