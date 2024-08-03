using AssetPerformanceToolkit.FrameBalancer;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssetPerformanceToolkit.AssetManagement
{
    public static class AssetLoader
    {
        public async static UniTask<AssetInstance> LoadInstance(AssetReference assetReference, Func<GameObject, Vector3, Quaternion, Transform, GameObject> action,
                                                                Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            var handler = Addressables.LoadAssetAsync<GameObject>(assetReference);

            await handler;

            if(handler.Status != AsyncOperationStatus.Succeeded)
            {
                Addressables.Release(handler);
                return new AssetInstance();
            }

            AssetInstance assetHolder = new AssetInstance(handler);
            var task = FrameTaskScheduler.Execute(() =>
            {
                GameObject go = action(handler.Result, position, rotation, parent);
                assetHolder.SetGo(go);
            });

            await task.Wait();

            return assetHolder;
        }

        public async static UniTask<AssetInstance> LoadInstance(AssetReference assetReference)
        {
            var handler = Addressables.LoadAssetAsync<GameObject>(assetReference);

            await handler;

            if (handler.Status != AsyncOperationStatus.Succeeded)
            {
                Addressables.Release(handler);
                return new AssetInstance();
            }

            AssetInstance assetHolder = new AssetInstance(handler);
            var task = FrameTaskScheduler.Execute(() =>
            {
                GameObject go = GameObject.Instantiate(handler.Result, Vector3.zero, Quaternion.identity, null);
                assetHolder.SetGo(go);
            });

            await task.Wait();

            return assetHolder;
        }
    }
}
