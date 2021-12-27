#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PaperWorks.Common.Animations;
using UnityEngine;

namespace PaperWorks.Workers
{
    public class WorkerHolderUI : IDisposable
    {
        protected readonly Vector2Int FirstElementPosition;
        protected readonly Vector2Int LastElementPosition;
        protected readonly WorkerHolder Holder;
        protected readonly float MoveTime;
        protected CancellationTokenSource Cancellation;

        public Vector2Int NewElementPosition => LastElementPosition;

        public WorkerHolderUI(
            Vector2Int firstElementPosition, Vector2Int lastElementPosition, WorkerHolder holder, float moveTime
        )
        {
            FirstElementPosition = firstElementPosition;
            LastElementPosition = lastElementPosition;
            Holder = holder;
            MoveTime = moveTime;

            Cancellation = new CancellationTokenSource();

            Holder.OnChange += HandleWorkersChanged;
        }
        
        public void Dispose() => Holder.OnChange -= HandleWorkersChanged;

        public void HandleWorkersChanged(IEnumerable<Transform> workers)
        {
            Cancellation.Cancel();
            Cancellation = new CancellationTokenSource();

            Action<float> tConsumer = (t) =>
            {
                int size = Mathf.Max(workers.Count() - 1, 1);
                int i = 0;

                foreach (Transform worker in workers)
                {
                    Vector2 end = Vector2.Lerp(FirstElementPosition, LastElementPosition, (float)i / size);

                    TConsumers.MovePosition(worker, (Vector2)worker.position, end, t);

                    i++;
                }
            };

            Interpolation.Interpolate(Cancellation.Token, MoveTime, tConsumer.Normalized(NormalizationFunctions.SmoothStep));
        }

        public void OnDrawGizmosSelected()
        {
            const int radius = 15;
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere((Vector2)FirstElementPosition, radius);
            Gizmos.DrawSphere((Vector2)LastElementPosition, radius);
            Gizmos.DrawLine((Vector2)FirstElementPosition, (Vector2)LastElementPosition);
        }
    }
}
