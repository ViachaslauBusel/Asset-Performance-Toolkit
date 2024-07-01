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
    public class TaskManager: MonoBehaviour
    {
        private const int TIME = 5;
        private static TaskManager _instance;
        private static Queue<IWorkTask> _tasks = new Queue<IWorkTask>();
        private static LinkedList<ICoroutineTask> _coroutines = new LinkedList<ICoroutineTask>();

        private void Awake()
        {
            if(_instance != null) { Debug.LogWarning("An instance of the \"ObjectLoader\" script has already been attached to the GameObject");  Destroy(gameObject); return; }
            _instance = this;
            enabled = false;
        }
        /// <summary>
        /// Add a task to the execution queue
        /// </summary>
        public static ITask Execute(Action action)
        {
            if(_instance == null)
            {
                Debug.LogError("The \"ObjectLoader\" script instance is not attached to the GameObject");
                return null;
            }

            Task task = new Task(action);
            _tasks.Enqueue(task);
          
            _instance.enabled = true;
            return task;
        }
        /// <summary>
        /// Load asset from bundle and add task to execution queue
        /// </summary>
        public static ITask Execute<T>(Prefab<T> prefab, Action<T> action) where T:UnityEngine.Object
        {
            if (_instance == null)
            {
                Debug.LogError("The \"ObjectLoader\" script instance is not attached to the GameObject");
                return null;
            }

            TaskPrefabLoader<T> task = new TaskPrefabLoader<T>(prefab, action);
            _coroutines.AddLast(task);

            _instance.enabled = true;
            return task;
        }
        /// <summary>
        /// Add a task to the execution queue
        /// </summary>
        public static ITask Execute(IEnumerator enumerator)
        {
            if (_instance == null)
            {
                Debug.LogError("The \"ObjectLoader\" script instance is not attached to the GameObject");
                return null;
            }
            TaskCoroutine task = new TaskCoroutine(enumerator);
            _coroutines.AddLast(task);
            _instance.enabled = true;
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

        internal static void InstantiateImmediately()
        {
           while(_tasks.Count > 0)
                 _tasks.Dequeue()?.Invoke();
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
