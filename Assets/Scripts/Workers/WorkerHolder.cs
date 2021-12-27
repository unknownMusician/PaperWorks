#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaperWorks.Workers
{
    public sealed class WorkerHolder
    {
        private readonly Queue<Transform> _workers = new Queue<Transform>();
        private readonly Transform _transform;

        public WorkerHolder(Transform transform) => _transform = transform;
        
        public event Action<IEnumerable<Transform>>? OnChange;

        public void Enqueue(Transform worker)
        {
            _workers.Enqueue(worker);
            worker.SetParent(_transform);

            HandleChange();
        }

        public Transform Dequeue()
        {
            Transform worker = _workers.Dequeue();
            worker.SetParent(null);

            HandleChange();

            return worker;
        }

        private void HandleChange() => OnChange?.Invoke(_workers);
    }
}
