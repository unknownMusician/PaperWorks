using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace PaperWorks.Common.Animations
{
    public static class Interpolation
    {
        public static IEnumerator Interpolate(float time, [NotNull] Action<float> tConsumer)
        {
            float t = 0.0f;

            while (t < 1.0f)
            {
                t += Time.deltaTime / time;
                tConsumer(t);

                yield return null;
            }

            tConsumer(t);
        }

        public static IEnumerator Interpolate(
            [NotNull] Cancellation cancellation, float time, [NotNull] Action<float> tConsumer
        )
        {
            float t = 0.0f;

            while (!cancellation.IsCanceled && t < 1.0f)
            {
                t += Time.deltaTime / time;
                tConsumer(t);

                yield return null;
            }

            tConsumer(t);
        }

        public static IEnumerator Interpolate(
            float time, [NotNull] Action<float> tConsumer, [NotNull] Action endHandler
        )
        {
            yield return Interpolation.Interpolate(time, tConsumer);

            endHandler();
        }

        public static IEnumerator Interpolate(
            [NotNull] Cancellation cancellation, float time, [NotNull] Action<float> tConsumer, [NotNull] Action endHandler
        )
        {
            yield return Interpolation.Interpolate(cancellation, time, tConsumer);

            while (!cancellation.IsCanceled)
            {
                endHandler();
            }
        }

        [NotNull]
        public static Action<float> Normalized(
            [NotNull] this Action<float> tConsumer, [NotNull] Func<float, float> normalizer
        )
            => (t) => tConsumer(normalizer(t));
    }
}
