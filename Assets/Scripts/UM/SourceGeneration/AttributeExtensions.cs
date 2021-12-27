#nullable enable

using System;
using System.Reflection;

namespace UM.SourceGeneration
{
    public static class AttributeExtensions
    {
        public static bool TryGetCustomAttribute<TAttribute>(this MemberInfo element, out TAttribute? attribute)
            where TAttribute : Attribute => (attribute = element.GetCustomAttribute<TAttribute>()) != null;
        
        public static bool TryGetCustomAttribute<TAttribute>(this ParameterInfo element, out TAttribute? attribute)
            where TAttribute : Attribute => (attribute = element.GetCustomAttribute<TAttribute>()) != null;
    }
}
