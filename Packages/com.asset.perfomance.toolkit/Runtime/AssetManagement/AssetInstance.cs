using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssetPerformanceToolkit.AssetManagement
{
    public class AssetInstance
    {
        private AsyncOperationHandle<GameObject> _assetHandle;
        private GameObject _objectInstance;

        public bool IsValid => _assetHandle.IsValid();
        public GameObject InstanceObject => _objectInstance;

        public AssetInstance(AsyncOperationHandle<GameObject> meshHandle)
        {
            _assetHandle = meshHandle;
        }

        public AssetInstance()
        {
        }

        public void Release()
        {
            if (_objectInstance != null)
            {
                GameObject.Destroy(_objectInstance);
            }
            if (_assetHandle.IsValid())
            {
                Addressables.Release(_assetHandle);
            }
        }

        internal void SetGo(GameObject go)
        {
            _objectInstance = go;
        }
    }
}
