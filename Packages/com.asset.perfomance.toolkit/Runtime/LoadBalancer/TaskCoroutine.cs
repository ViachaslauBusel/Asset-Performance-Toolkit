using System.Collections;

namespace OpenWorld.Loader
{
    public class TaskCoroutine : ITask, IWorkTask, ICoroutineTask
    {
        private IEnumerator m_enumerator;
        public bool IsCompleted { get; private set; } = false;

        public TaskCoroutine(IEnumerator enumerator)
        {
            m_enumerator = enumerator;
        }

        public void Cancel()
        {
            IsCompleted = true;
        }

        public void Invoke()
        {
            IsCompleted = true;
        }

        public bool MoveNext()
        {
            if (IsCompleted) { return false; }
            return m_enumerator.MoveNext();
        }
    }
}
