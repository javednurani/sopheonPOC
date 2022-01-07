namespace Sopheon.CloudNative.Products.Domain
{
   public class EntityChangeEvent<T>
   {
      public EntityChangeEventTypes EntityChangeEventType { get; set; }
      public DeltaPair<T> PreValue { get; set; }
      public DeltaPair<T> PostValue { get; set; }
      public DateTime Timestamp { get; set; }
   }
}