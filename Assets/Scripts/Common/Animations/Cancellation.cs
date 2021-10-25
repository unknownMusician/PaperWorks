namespace PaperWorks.Common.Animations
{
    public sealed class Cancellation
    {
        internal bool IsCanceled;

        public void Cancel() => IsCanceled = true;
    }
}
