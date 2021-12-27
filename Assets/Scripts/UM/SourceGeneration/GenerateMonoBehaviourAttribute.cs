#nullable enable

using System;
using UnityEngine.Scripting;

namespace UM.SourceGeneration
{
    // todo: struct?
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GenerateMonoBehaviourAttribute : PreserveAttribute { }
    
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromThisObjectAttribute : PreserveAttribute
    {
        public readonly bool GetSpecificType;

        public FromThisObjectAttribute(bool getSpecificType = false)
        {
            GetSpecificType = getSpecificType;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class InjectAttribute : PreserveAttribute
    {
        public readonly Type InjectedType;
        
        public InjectAttribute(Type injectedType) => InjectedType = injectedType;
    }

    // public sealed class ComponentForAttribute : Attribute
    // {
    //     private readonly Type _type;
    //
    //     public ComponentForAttribute(Type type) => _type = type;
    // }
}
