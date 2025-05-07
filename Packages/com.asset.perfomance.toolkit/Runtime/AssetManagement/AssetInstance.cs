using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssetPerformanceToolkit.AssetManagement
{
    public class AssetInstance : AssetInstance<GameObject>
    {
        private GameObject _objectInstance;
     
        public GameObject InstanceObject => _objectInstance;

        public AssetInstance(AsyncOperationHandle<GameObject> meshHandle) : base(meshHandle)
        {
        }

        public AssetInstance(GameObject go)
        {
            _objectInstance = go;
        }

        public AssetInstance()
        {
        }

        public override void Release()
        {
            if (_objectInstance != null)
            {
                GameObject.Destroy(_objectInstance);
            }
            base.Release();
        }

        internal void SetGo(GameObject go)
        {
            _objectInstance = go;
        }
    }

    public class AssetInstance<T> where T : UnityEngine.Object
    {
        private AsyncOperationHandle<T> _assetHandle;

        public bool IsValid => _assetHandle.IsValid();
        public T Asset => _assetHandle.Result;

        public AssetInstance(AsyncOperationHandle<T> assetHandle)
        {
            _assetHandle = assetHandle;
        }

        public AssetInstance()
        {
        }

        public virtual void Release()
        {
            if (_assetHandle.IsValid())
            {
                Addressables.Release(_assetHandle);
            }
        }
    }
}
