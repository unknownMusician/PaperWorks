using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using PaperWorks.Common;
using UnityEngine;

namespace PaperWorks.Workers
{
    public sealed class WorkerHolderUI : MonoBehaviour
    {
        [SerializeField] private Vector2Int _firstElementPosition;
        [SerializeField] private Vector2Int _lastElementPosition;
        [SerializeField] private WorkerHolder _holder;

        private void OnValidate() => this.AssertNotNull(_holder);

        private void Awake() => _holder.OnChange += HandleWorkersChanged;
        private void OnDestroy() => _holder.OnChange -= HandleWorkersChanged;

        public void HandleWorkersChanged([NotNull] IEnumerable<Transform> workers)
        {
            int size = workers.Count();
            int i = 0;
            
            foreach (Transform worker in workers)
            {
                worker.position = Vector2.Lerp(_firstElementPosition, _lastElementPosition, (float)i / size);
                i++;
            }
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
