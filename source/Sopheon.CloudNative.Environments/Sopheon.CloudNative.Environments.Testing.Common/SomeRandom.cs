﻿using System;
using System.Text;

namespace Sopheon.CloudNative.Environments.Testing.Common
{
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

   public class SomeRandom
   {
      private const string _ALPHA_NUMERIC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      private static readonly Random _random = new Random();

      public string String()
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
   }
}
