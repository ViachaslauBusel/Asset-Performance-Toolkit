using Cysharp.Threading.Tasks;
using System;

namespace AssetPerformanceToolkit.FrameBalancer
{
    public class Task : ITask, IWorkTask
    {
        private Action m_action;
        public bool IsCompleted { get; private set; } = false;

        public Task(Action action)
        {
            this.m_action = action;
        }

        public void Invoke()
        {
            m_action?.Invoke();
            IsCompleted = true;
        }

        public void Cancel()
        {
            m_action = null;
            IsCompleted = true;
        }

        public UniTask Wait()
        {
            return UniTask.WaitUntil(() => IsCompleted);
        }
    }
}