using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

#if UNITY_EDITOR
namespace UM.SourceGeneration
{
    //[InitializeOnLoad]
    internal static class SourceGeneratorInitializer
    {
        static SourceGeneratorInitializer()
        {
            AssemblyReloadEvents.beforeAssemblyReload += Start;
            Start();
        }

        private static void Start()
        {
            new SourceGenerator(new FileWriter(), new SourceGeneratorUtil()).Inspect(AssemblyHelper.GameAssembly);
        }
    }

    public static class AssemblyHelper
    {
        public static readonly Assembly GameAssembly = AppDomain.CurrentDomain.GetAssemblies()
                                                                .First(
                                                                    assembly => assembly.GetName().Name
                                                                     == "Assembly-CSharp"
                                                                );
    }
}
#endif
