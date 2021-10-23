using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PaperWorks.Workers
{
    public sealed class WorkerHolder : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [NotNull] private readonly Queue<Transform> _workers = new Queue<Transform>();

        public event Action<IEnumerable<Transform>> OnChange;

        // todo
        private IEnumerator Start()
        {
            while (true)
            {
                Enqueue(Object.Instantiate(_prefab).transform);

                yield return new WaitForSeconds(1.0f);
            }
        }

        public void Enqueue([NotNull] Transform worker)
        {
            _workers.Enqueue(worker);
            worker.SetParent(transform);

            HandleChange();
        }

        [NotNull]
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
