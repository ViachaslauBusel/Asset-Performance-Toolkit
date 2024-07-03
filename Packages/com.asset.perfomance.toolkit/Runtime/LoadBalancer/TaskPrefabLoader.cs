using Bundles;
using DATA;
using System;
using System.Collections;
using UnityEngine;

namespace OpenWorld.Loader
{
    class TaskPrefabLoader<T> : ITask, IWorkTask, ICoroutineTask where T: UnityEngine.Object
    {
        private IEnumerator _enumerator;
        private T _prefab;
        private Action<T> _action;
        private bool _isCompleted = false;


        public bool IsCompleted => _isCompleted;


        public TaskPrefabLoader(Prefab<T> prefab, Action<T> action) 
        {
            _enumerator = PrefabLoader(prefab);
            _action = action;
        }

        public void Cancel()
        {
            _enumerator = null;
            _action = null;
            _isCompleted = true;
        }

        public void Invoke()
        {
            _action?.Invoke(_prefab);
            _isCompleted = true;
        }

        public bool MoveNext()
        {
            if (_enumerator == null) return false;
            return _enumerator.MoveNext();
        }

        private IEnumerator PrefabLoader(Prefab<T> prefab)
        {
            if (prefab == null) yield break;
#if UNITY_EDITOR
            _prefab = prefab.Asset;
            yield return null;
#else
            var request = BundlesManager.LoadAssetAsync(prefab);
            if (request == null)
            {
                Debug.LogError("Can't find Bundle");
                yield break;
            }
        yield return request;
        _prefab = request.asset as T;
        if(_prefab == null) Debug.Log($"Error in wait or cast:{request.asset == null} : {request.asset.GetType()}");
#endif
        }
    }
}
