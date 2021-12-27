using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

#nullable enable

namespace UM.SourceGeneration
{
    public class Generator
    {
        public string CreateTypeBody(GeneratedType type)
        {
            string classText = $@"
namespace {type.Namespace}{{
    {type.AccessModifier.ToModifier()} {(type.IsStatic ? "static " : " ")}{type.TypeType.ToType()} {type.Name} : {type.Inheritee.AggregateWithComma()}
    {{
{ToBody(type.Fields, 2)}

    }}";

            return classText;
        }

        public string ToBody(GeneratedField[] fields, int indent)
        {
            string tabs = string.Empty;

            for (int i = 0; i < indent; i++)
            {
                tabs += "    ";
            }

            return fields.Select(
                             field => tabs
                                    + ToBody(field)
                                    + @";
"
                         )
                         .Aggregate((a, b) => a + b);
        }

        public string ToBody(GeneratedField field)
        {
            string result = string.Empty;

            if (field.Attributes.Any())
            {
                result += $"[{field.Attributes.AggregateWithComma()}] ";
            }

            result += $"{field.AccessModifier.ToModifier()} ";

            if (field.IsConst)
            {
                result += "const ";
            }

            if (field.IsStatic)
            {
                result += "static ";
            }

            if (field.IsReadonly)
            {
                result += "readonly ";
            }

            result += $"{field.Type} {field.Name};";

            return result;
        }
    }

    public static class Util
    {
        public static string AggregateWithComma(this IEnumerable<string> words)
        {
            return words.Aggregate((type1, type2) => $"{type1}, {type2}");
        }
    }

    public struct GeneratedType
    {
        public string Name;
        public string Namespace;
        public AccessModifier AccessModifier;
        public TypeType TypeType;
        public bool IsStatic;
        public string[] Inheritee;
        public GeneratedField[] Fields;
    }

    public struct GeneratedField
    {
        public string Name;
        public Type Type;
        public AccessModifier AccessModifier;
        public TypeType TypeType;
        public bool IsStatic;
        public bool IsReadonly;
        public bool IsConst;
        public string[] Attributes;
    }

    public struct GeneratedMethod
    {
        public string Name;
        public Type ReturnType;
        public ParameterInfo[] Parameters;
        public string Body;
        public AccessModifier AccessModifier;
    }

    public enum AccessModifier
    {
        Public,
        Private,
        Protected,
        Internal,
        PrivateProtected,
        ProtectedInternal,
    }

    public enum TypeType { Interface, Class, Struct, }

    public static class AccessModifierExtensions
    {
        public static string ToModifier(this AccessModifier modifier) => modifier switch
        {
            AccessModifier.Public => "public",
            AccessModifier.Private => "private",
            AccessModifier.Protected => "protected",
            AccessModifier.Internal => "internal",
            AccessModifier.PrivateProtected => "private protected",
            AccessModifier.ProtectedInternal => "protected internal",
            _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
        };
    }

    public static class TypeTypeExtensions
    {
        public static string ToType(this TypeType type) => type switch
        {
            TypeType.Interface => "interface",
            TypeType.Class => "class",
            TypeType.Struct => "struct",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
