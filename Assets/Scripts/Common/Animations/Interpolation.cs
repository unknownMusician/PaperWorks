#nullable enable

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace PaperWorks.Common.Animations
{
    public static class Interpolation
    {
        public static async Task Interpolate(float time, Action<float> tConsumer)
        {
            float t = 0.0f;

            while (t < 1.0f)
            {
                t += Time.deltaTime / time;
                tConsumer(t);

                await Task.Yield();
            }

            tConsumer(t);
        }

        public static async Task Interpolate(CancellationToken cancellation, float time, Action<float> tConsumer)
        {
            float t = 0.0f;

            while (!cancellation.IsCancellationRequested && t < 1.0f)
            {
                t += Time.deltaTime / time;
                tConsumer(t);

                await Task.Yield();
            }

            tConsumer(t);
        }

        public static async Task  Interpolate(float time, Action<float> tConsumer, Action endHandler)
        {
            await Interpolate(time, tConsumer);

            endHandler();
        }

        public static async Task  Interpolate(
            CancellationToken cancellation, float time, Action<float> tConsumer, Action endHandler
        )
        {
            await Interpolation.Interpolate(cancellation, time, tConsumer);

            while (!cancellation.IsCancellationRequested)
            {
                endHandler();
            }
        }

        public static Action<float> Normalized(this Action<float> tConsumer, Func<float, float> normalizer)
            => (t) => tConsumer(normalizer(t));
    }
}
