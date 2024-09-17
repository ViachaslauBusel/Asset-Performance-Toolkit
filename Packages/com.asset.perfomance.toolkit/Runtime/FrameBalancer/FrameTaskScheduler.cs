using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetPerformanceToolkit.FrameBalancer
{
    /// <summary>
    /// Conveyer of tasks, controls the execution time, dividing tasks by frames
    /// </summary>
    public partial class FrameTaskScheduler : MonoBehaviour
    {
        private const int TIME = 1;

        private Queue<IWorkTask> _tasks = new Queue<IWorkTask>();

        /// <summary>
        /// Add an action task to the execution queue
        /// </summary>
        public ITask ScheduleTask(Action action)
        {
            Task task = new Task(action);
            _tasks.Enqueue(task);

            enabled = true;
            return task;
        }

        private void Update()
        {
            if (_tasks.Count > 0)
            {
                long timeStart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                int taskDone = 0;

                while (_tasks.Count > 0 && (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timeStart) < TIME) 
                {
                    try
                    {
                        _tasks.Dequeue().Invoke();
                        taskDone++;
                    }
                    catch (Exception e) { Debug.LogError($"Error in Task: {e}"); }
                }
            }
            else enabled = false;
        }

        public void ExecuteAllTasksImmediately()
        {
           while(_tasks.Count > 0)
                 _tasks.Dequeue()?.Invoke();
        }
    }
}
