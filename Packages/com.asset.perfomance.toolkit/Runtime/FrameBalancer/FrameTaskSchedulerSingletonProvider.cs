using System;
using UnityEngine;

namespace AssetPerformanceToolkit.FrameBalancer
{
    /// <summary>
    /// Conveyer of tasks, controls the execution time, dividing tasks by frames
    /// </summary>
    public partial class FrameTaskScheduler : MonoBehaviour
    {
        private static FrameTaskScheduler _instance;

        private static FrameTaskScheduler Instance
        {
            get
            {
                if (_instance == null)
                {
                    if(!Application.isPlaying)
                    {
                        Debug.LogError("FrameTaskScheduler is not available in edit mode");
                        return null;
                    }
                    GameObject go = new GameObject("FrameTaskScheduler");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<FrameTaskScheduler>();
                    _instance.enabled = false;
                }
                return _instance;
            }
        }

        public static ITask Execute(Action action)
        {
            if (Instance == null)
            {
                Task task = new Task(action);
                task.Invoke();
                return task;
            }
            return Instance.ScheduleTask(action);
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
