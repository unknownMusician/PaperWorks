#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Object = System.Object;

namespace UM.SourceGeneration
{
    public class SourceGeneratorUtil
    {
        public virtual string GetAliasName(Type type) => GetAliasName(
            type.FullName ?? throw new ArgumentException($"Cannot get FullName of type {type.Name}")
        );

        public virtual string GetAliasName(string typeFullName) => typeFullName switch
        {
            nameof(System) + "." + nameof(Boolean) => "string",
            nameof(System) + "." + nameof(Byte) => "byte",
            nameof(System) + "." + nameof(SByte) => "sbyte",
            nameof(System) + "." + nameof(Char) => "char",
            nameof(System) + "." + nameof(Decimal) => "decimal",
            nameof(System) + "." + nameof(Double) => "double",
            nameof(System) + "." + nameof(Single) => "float",
            nameof(System) + "." + nameof(Int32) => "int",
            nameof(System) + "." + nameof(UInt32) => "uint",
            //nameof(System) + "." + nameof(IntPtr) => "nint",
            //nameof(System) + "." + nameof(UIntPtr) => "nuint",
            nameof(System) + "." + nameof(Int64) => "long",
            nameof(System) + "." + nameof(UInt64) => "ulong",
            nameof(System) + "." + nameof(Int16) => "short",
            nameof(System) + "." + nameof(UInt16) => "ushort",
            nameof(System) + "." + nameof(Object) => "object",
            nameof(System) + "." + nameof(String) => "string",
            _ => "global::" + typeFullName,
        };
    }

    public class SourceGenerator
    {
        protected readonly FileWriter Writer;
        protected readonly SourceGeneratorUtil _util;

        public SourceGenerator(FileWriter writer, SourceGeneratorUtil util)
        {
            Writer = writer;
            _util = util;
        }

        // todo
        public virtual IEnumerable<Type> GetInspectedTypes(Assembly assembly) => assembly.GetTypes()
            .Where(type => type.TryGetCustomAttribute(out GenerateMonoBehaviourAttribute _));

        public virtual void HandleType(Type type)
        {
            Writer.WriteAllText(Writer.GetFilePath(type, "Component"), GenerateMonoBehaviour(type));
        }

        public virtual void Inspect(Assembly assembly)
        {
            foreach (Type type in GetInspectedTypes(assembly))
            {
                HandleType(type);
            }
        }

        public virtual string GenerateMonoBehaviour(
            string @namespace, string typeName, bool isDisposable, bool isTriggerEnterHandler, ParameterInfo[] fields,
            ParameterInfo[] parameters
        ) => GenerateMonoBehaviour(
            @namespace,
            typeName,
            isDisposable,
            isTriggerEnterHandler,
            GetParametersAsFields(fields),
            GetParametersAsInvocationParameters(parameters)
        );

        public virtual string GenerateMonoBehaviour(
            string @namespace, string typeName, bool isDisposable, bool isTriggerEnterHandler, string fields, string parameters
        )
        {
            return $@"// Generated

namespace Generated.{@namespace}
{{
    public sealed class {typeName}Component : global::UnityEngine.MonoBehaviour, global::{typeof(IComponent<>).Namespace}.IComponent<global::{@namespace}.{typeName}>
    {{{fields}
        public global::{@namespace}.{typeName} HeldType {{ get; private set; }}

        private void Awake()
        {{
            HeldType = new global::{@namespace}.{typeName}({parameters});
        }}{(!isTriggerEnterHandler ? string.Empty : $@"
            
        private void OnTriggerEnter(global::UnityEngine.Collider other)
        {{
            HeldType.OnTriggerEnter(other);
        }}")}{(!isDisposable ? string.Empty : $@"

        private void OnDestroy()
        {{
            HeldType.Dispose();
        }}")}
    }}
}}
";
        }

        public virtual string GenerateMonoBehaviour(Type type)
        {
            if (type.IsAbstract)
            {
                throw new ArgumentException(
                    $"Cannot generate MonoBehaviour for {type.Name} because it is abstract."
                );
            }

            if (string.IsNullOrEmpty(type.Namespace))
            {
                throw new NotImplementedException(
                    $"Cannot generate MonoBehaviour for {type.Name} because it has no namespace."
                );
            }

            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            if (constructors.Length > 1)
            {
                throw new ArgumentException(
                    $"Cannot generate MonoBehaviour for {type.Name} because it has more than one constructor."
                );
            }

            ParameterInfo[] constructorParameters = Array.Empty<ParameterInfo>();

            if (constructors.Length == 1)
            {
                constructorParameters = constructors.First().GetParameters();
            }

            return GenerateMonoBehaviour(
                type.Namespace!,
                type.Name,
                typeof(IDisposable).IsAssignableFrom(type),
                typeof(ITriggerEnterHandler).IsAssignableFrom(type),
                constructorParameters,
                constructorParameters
            );
        }

        protected virtual string GetParametersAsInvocationParameters(ParameterInfo[] parameters)
        {
            var invocation = new StringBuilder();

            foreach (ParameterInfo parameter in parameters)
            {
                invocation.Append($"{GetParameterAsInvocationParameter(parameter)}, ");
            }

            if (invocation.Length > 0)
            {
                invocation.Remove(invocation.Length - 2, 2);
            }

            return invocation.ToString();
        }

        protected virtual string GetParameterAsInvocationParameter(ParameterInfo parameter)
        {
            string parameterName = '_' + parameter.Name;

            Type parameterType = parameter.ParameterType;

            if (parameter.TryGetCustomAttribute(out InjectAttribute? injectAttribute))
            {
                if (!parameterType.IsInterface)
                {
                    throw new NotImplementedException(
                        $"{parameterType.Name} cannot be injected as it is not an interface."
                    );
                }

                parameterType = injectAttribute!.InjectedType;
            }

            if (parameterType.TryGetCustomAttribute(out GenerateMonoBehaviourAttribute _))
            {
                parameterName += ".HeldType";
            }

            if (parameter.TryGetCustomAttribute(out FromThisObjectAttribute? fromThisObjectAttribute))
            {
                if (parameter.ParameterType == typeof(GameObject))
                {
                    parameterName = "gameObject";
                }
                else if (parameter.ParameterType == typeof(Transform))
                {
                    parameterName = "transform";
                }
                else if (fromThisObjectAttribute!.GetSpecificType)
                {
                    parameterName = $"GetComponent<{_util.GetAliasName(parameterType)}>()";
                }
                else
                {
                    parameterName = $"GetComponent<global::{typeof(IComponent<>).Namespace}.IComponent<{_util.GetAliasName(parameterType)}>>().HeldType";
                }
            }

            return parameterName;
        }

        protected virtual string GetParametersAsFields(ParameterInfo[] parameters)
        {
            var fields = new StringBuilder();

            foreach (ParameterInfo parameter in parameters)
            {
                if (TryGetParameterAsField(parameter, out string? result))
                {
                    fields.Append(
                        $@"
        {result!}"
                    );
                }
            }

            if (fields.Length > 0)
            {
                fields.Append(
                    @"
"
                );
            }

            return fields.ToString();
        }

        protected virtual bool TryGetParameterAsField(ParameterInfo parameter, out string? result)
        {
            if (parameter.TryGetCustomAttribute(out FromThisObjectAttribute _))
            {
                result = null;

                return false;
            }

            Type parameterType = parameter.ParameterType;

            if (parameter.TryGetCustomAttribute(out InjectAttribute? attribute))
            {
                if (!parameterType.IsInterface)
                {
                    throw new NotImplementedException(
                        $"{parameterType.Name} cannot be injected as it is not an interface."
                    );
                }

                parameterType = attribute!.InjectedType;
            }

            string typeFullName = parameterType.FullName
                               ?? throw new ArgumentException($"Type {parameterType.Name} has no FullName.");

            if (parameterType.TryGetCustomAttribute(out GenerateMonoBehaviourAttribute _))
            {
                typeFullName = $"Generated.{typeFullName}Component";
            }

            if (parameterType.IsGenericType)
            {
                typeFullName =
                    $"{parameterType.FullName.Substring(0, parameterType.FullName.IndexOf('`'))}<{GenericTypeArgumentsToString(parameterType)}>";
            }

            typeFullName = _util.GetAliasName(typeFullName);

            result = $"[global::UnityEngine.SerializeField] private {typeFullName} _{parameter.Name};";

            return true;
        }

        protected virtual string GenericTypeArgumentsToString(Type type)
        {
            var builder = new StringBuilder();

            foreach (Type argument in type.GenericTypeArguments)
            {
                builder.Append(_util.GetAliasName(argument) + ", ");
            }

            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 2, 2);
            }

            return builder.ToString();
        }
    }
}
