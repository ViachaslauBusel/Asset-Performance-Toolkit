using System;
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
        private static TaskManager _instance;

        private static TaskManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    if(!Application.isPlaying)
                    {
                        return null;
                    }
                    GameObject go = new GameObject("TaskManager");
                    _instance = go.AddComponent<TaskManager>();
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
            return Instance.ExecuteAction(action);
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
