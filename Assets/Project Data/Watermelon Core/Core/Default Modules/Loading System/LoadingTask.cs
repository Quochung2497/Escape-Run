namespace Watermelon
{
    public abstract class LoadingTask
    {
        protected bool isActive;
        public bool IsActive { get => isActive; }

        protected bool isFinished;
        public bool IsFinished { get => isFinished; }

        public event SimpleCallback OnTaskCompleted;

        public LoadingTask()
        {
            isActive = false;
            isFinished = false;
        }

        public void CompleteTask()
        {
            isFinished = true;

            OnTaskCompleted?.Invoke();
        }

        public abstract void Activate();
    }
}
