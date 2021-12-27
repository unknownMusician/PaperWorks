using PaperWorks.Workers;
using UM.ComponentGeneration;

namespace PaperWorks.Components.Workers
{
    public sealed class WorkerHolderComponent : GeneratedComponent<WorkerHolder>
    {
        protected override WorkerHolder Create()
        {
            return new WorkerHolder(transform);
        }
    }
}
