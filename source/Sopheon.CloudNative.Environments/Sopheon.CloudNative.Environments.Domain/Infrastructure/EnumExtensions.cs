using System;
using System.Reflection;

namespace Sopheon.CloudNative.Environments.Domain.Infrastructure
{
   public static class EnumExtensions
   {
      public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
      {
         Type type = value.GetType();
         string name = Enum.GetName(type, value);
         return type.GetField(name)
             .GetCustomAttribute<TAttribute>();
      }
   }
}
