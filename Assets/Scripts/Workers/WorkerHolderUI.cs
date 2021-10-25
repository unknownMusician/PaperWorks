using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using JetBrains.Annotations;
using PaperWorks.Common;
using PaperWorks.Common.Animations;
using UnityEngine;

namespace PaperWorks.Workers
{
    public sealed class WorkerHolderUI : MonoBehaviour
    {
        [SerializeField] private Vector2Int _firstElementPosition;
        [SerializeField] private Vector2Int _lastElementPosition;
        [SerializeField] private WorkerHolder _holder;
        [SerializeField] private float _moveTime;

        private Cancellation _cancellation = new Cancellation();

        public Vector2Int NewElementPosition => _lastElementPosition;

        private void OnValidate() => this.AssertNotNull(_holder);

        private void Awake() => _holder.OnChange += HandleWorkersChanged;
        private void OnDestroy() => _holder.OnChange -= HandleWorkersChanged;

        public void HandleWorkersChanged([NotNull] IEnumerable<Transform> workers)
        {
            _cancellation.Cancel();
            _cancellation = new Cancellation();

            Action<float> tConsumer = (t) =>
            {
                int size = Mathf.Max(workers.Count() - 1, 1);
                int i = 0;

                foreach (Transform worker in workers)
                {
                    Vector2 end = Vector2.Lerp(_firstElementPosition, _lastElementPosition, (float)i / size);

                    TConsumers.MovePosition(worker, (Vector2)worker.position, end, t);

                    i++;
                }
            };

            StartCoroutine(Interpolation.Interpolate(_cancellation, _moveTime, tConsumer.Normalized(NormalizationFunctions.SmoothStep)));
        }

        private IEnumerator Test2(out int g)
        {
            g = 5;

            return Test1();
        }

        private IEnumerator Test1()
        {
            yield return 1;
        }

        private void OnDrawGizmosSelected()
        {
            const int radius = 15;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere((Vector2)_firstElementPosition, radius);
            Gizmos.DrawSphere((Vector2)_lastElementPosition, radius);
            Gizmos.DrawLine((Vector2)_firstElementPosition, (Vector2)_lastElementPosition);
        }
    }
}
