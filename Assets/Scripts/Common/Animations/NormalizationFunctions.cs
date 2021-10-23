using UnityEngine;

namespace PaperWorks.Common.Animations
{
    public static class NormalizationFunctions
    {
        public static float Linear(float t) => t;
        public static float SmoothStep(float t) => Mathf.SmoothStep(0.0f, 1.0f, t);
    }
}
