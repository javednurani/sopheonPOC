using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Sopheon.CloudNative.Environments.Domain.Models;

[assembly: ExcludeFromCodeCoverage]
namespace Sopheon.CloudNative.Environments.Testing.Common
{
   /// <summary>
   /// This class exists as a wrapper around SomeRandom to allow easy, fluent access
   /// </summary>
   public static class Some
   {
      public static SomeRandom Random
      {
         get
         {
            return new SomeRandom();
         }
      }
   }

   /// <summary>
   /// The purpose of this class is to generate random primative data.
   /// Instance exists to allow extension with domain-specic random data
   /// </summary>
   public class SomeRandom
   {
      private const string _ALPHA_NUMERIC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      private static readonly Random _random = new Random();

      public string String(int length = 32)
      {
         StringBuilder stringBuilder = new StringBuilder();

         for (int i = 0; i < length; i++)
         {
            int index = _random.Next(0, _ALPHA_NUMERIC.Length);
            char character = _ALPHA_NUMERIC[index];
            stringBuilder.Append(character);
         }

         return stringBuilder.ToString();
      }

      public int Int()
      {
         return _random.Next();
      }

      public Guid Guid()
      {
         return System.Guid.NewGuid();
      }

      public T Enum<T>() where T : Enum
      {
         Array enumValues = System.Enum.GetValues(typeof(T));
         int index = _random.Next(enumValues.Length);
         return (T)enumValues.GetValue(index);
      }

      public Resource Resource()
      {
         return new Resource
         {
            Uri = String(),
            Name = String(),
            IsAssigned = false
         };
      }
   }
}
