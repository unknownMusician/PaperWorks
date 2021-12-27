using PaperWorks.Common;
using PaperWorks.Workers;
using UM.Assertions;
using UM.ComponentGeneration;
using UnityEngine;

namespace PaperWorks.Components.Workers
{
    public sealed class WorkerHolderUIComponent : GeneratedComponent<WorkerHolderUI>
    {
        [SerializeField] private Vector2Int _firstElementPosition;
        [SerializeField] private Vector2Int _lastElementPosition;
        [SerializeField] private WorkerHolderComponent _holder;
        [SerializeField] private float _moveTime;

        protected override WorkerHolderUI Create()
        {
            _holder.AssertNotNull();

            return new WorkerHolderUI(_firstElementPosition, _lastElementPosition, _holder.HeldItem, _moveTime);
        }

        private void OnDestroy() => HeldItem?.Dispose();
        private void OnDrawGizmosSelected() => HeldItem?.OnDrawGizmosSelected();
    }
}
