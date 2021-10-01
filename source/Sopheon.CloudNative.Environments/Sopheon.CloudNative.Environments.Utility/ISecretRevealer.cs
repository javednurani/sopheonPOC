namespace Sopheon.CloudNative.Environments.Utility
{
   public interface ISecretRevealer
   {
      string RevealLocalConnectionString();
   }
}
