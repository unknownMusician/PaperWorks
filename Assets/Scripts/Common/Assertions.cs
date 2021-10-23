using System;
using JetBrains.Annotations;
using UnityEngine;

namespace PaperWorks.Common
{
    public static class Assertions
    {
        public static void AssertNotNull<TNullable>(
            [CanBeNull] this TNullable nullable, [CanBeNull] string name = ""
        ) where TNullable : class
        {
            if (nullable == null)
            {
                string valueName = string.IsNullOrEmpty(name) ? "value" : name;

                throw new ArgumentNullException($"{valueName} should not be null");
            }
        }

        public static void AssertNotNull<TBehaviour>(
            [NotNull] this TBehaviour behaviour, [NotNull] params object[] nullables
        ) where TBehaviour : MonoBehaviour
        {
            int i = 0;

            foreach (object nullable in nullables)
            {
                AssertNotNull(nullable, $"Value #{i} in {behaviour.GetType().Name}");

                i++;
            }
        }
    }
}
