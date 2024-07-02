using DATA;
using System.Collections.Generic;
using UnityEngine;

namespace Bundles
{
    public static class BundlesManager 
    {
        public static bool IsReady { get; private set; } = false;
        private static List<AssetBundle> m_bundles = new List<AssetBundle>();
        private static Dictionary<string, AssetBundle> m_bundlesStorage = new Dictionary<string, AssetBundle>();



        public static void Add(AssetBundle bundle)
        {
            m_bundlesStorage.Add(bundle.name, bundle);
            m_bundles.Add(bundle);
        }
        public static void LoadingComplete() => IsReady = true;


        public static AssetBundleRequest LoadAssetAsync(IPrefab prefab) 
        {
            if (!m_bundlesStorage.TryGetValue(prefab.Bundle, out AssetBundle bundle)) return null;
             return bundle.LoadAssetAsync(prefab.Path);
        }


        public static void Clear()
        {
            foreach(AssetBundle bundle in m_bundles) { bundle.Unload(true); }
            m_bundles.Clear();
            m_bundlesStorage.Clear();
            IsReady = false;
        }
    }
}