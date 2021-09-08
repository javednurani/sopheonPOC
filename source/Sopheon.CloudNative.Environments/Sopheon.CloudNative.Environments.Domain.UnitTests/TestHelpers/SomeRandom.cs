using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sopheon.CloudNative.Environments.Domain.UnitTests.TestHelpers
{
   public static class SomeRandom
   {
      private const string _ALPHA_NUMERIC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      private static readonly Random _random = new Random();

      public static string String()
      {
         StringBuilder stringBuilder = new StringBuilder();

         for (int i = 0; i < 32; i++)
         {
            int index = _random.Next(0, _ALPHA_NUMERIC.Length);
            char character = _ALPHA_NUMERIC[index];
            stringBuilder.Append(character);
         }

         return stringBuilder.ToString();
      }

      public static int Int()
      {
         return _random.Next();
      }

      public static Guid Guid()
      {
         return System.Guid.NewGuid();
      }

      public static T Enum<T>() where T : Enum
      {
         Array enumValues = System.Enum.GetValues(typeof(T));
         int index = _random.Next(enumValues.Length);
         return (T)enumValues.GetValue(index);
      }
   }
}
