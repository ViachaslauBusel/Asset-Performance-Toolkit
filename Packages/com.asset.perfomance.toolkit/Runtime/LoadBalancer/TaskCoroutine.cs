using System.Collections;

namespace OpenWorld.Loader
{
    class TaskCoroutine : ITask, IWorkTask, ICoroutineTask
    {
        private IEnumerator m_enumerator;
        public bool Completed { get; private set; } = false;

        public TaskCoroutine(IEnumerator enumerator)
        {
            m_enumerator = enumerator;
        }

        public void Cancel()
        {
            Completed = true;
        }

        public void Invoke()
        {
            Completed = true;
        }

        public bool MoveNext()
        {
            if (Completed) { return false; }
            return m_enumerator.MoveNext();
        }
    }
}
