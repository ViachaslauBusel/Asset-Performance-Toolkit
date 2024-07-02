using DATA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenWorld.Loader
{
    /// <summary>
    /// Conveyer of tasks, controls the execution time, dividing tasks by frames
    /// </summary>
    public partial class TaskManager : MonoBehaviour
    {
        private const int TIME = 5;

        private Queue<IWorkTask> _tasks = new Queue<IWorkTask>();
        private LinkedList<ICoroutineTask> _coroutines = new LinkedList<ICoroutineTask>();
      

        /// <summary>
        /// Add a task to the execution queue
        /// </summary>
        public ITask Execute(Action action)
        {
            Task task = new Task(action);
            _tasks.Enqueue(task);
          
            enabled = true;
            return task;
        }

        /// <summary>
        /// Load asset from bundle and add task to execution queue
        /// </summary>
        public ITask Execute<T>(Prefab<T> prefab, Action<T> action) where T:UnityEngine.Object
        {
            TaskPrefabLoader<T> task = new TaskPrefabLoader<T>(prefab, action);
            _coroutines.AddLast(task);

            enabled = true;
            return task;
        }

        /// <summary>
        /// Add a task to the execution queue
        /// </summary>
        public ITask Execute(IEnumerator enumerator)
        {

            TaskCoroutine task = new TaskCoroutine(enumerator);
            _coroutines.AddLast(task);
            enabled = true;
            return task;
        }

        private void Update()
        {
            if (_tasks.Count > 0 || _coroutines.Count > 0)
            {
                long timeStart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var node = _coroutines.First;
                while(node != null)
                {
                    var next = node.Next;
                    if (!node.Value.MoveNext())
                    {
                        _tasks.Enqueue((IWorkTask)node.Value);
                        _coroutines.Remove(node);
                    }
                    node = next;
                }

                while (_tasks.Count > 0 && (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timeStart) < TIME) 
                {
                    try
                    {
                        _tasks.Dequeue().Invoke();
                    }
                    catch (Exception e) { Debug.LogError($"Error in Task: {e}"); }
                }
            }
            else enabled = false;
        }

        public void InstantiateImmediately()
        {
           while(_tasks.Count > 0)
                 _tasks.Dequeue()?.Invoke();
        }
    }
}
