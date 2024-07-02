using System;
using UnityEditor;
using UnityEngine;

namespace DATA
{
    /// <summary>
    /// Wrapper for an asset, used for loading from bundles.
    /// </summary>
    [Serializable]
    public class Prefab<T> :  ISerializationCallbackReceiver, IPrefab where T : UnityEngine.Object
    {
        [SerializeField] string _guid;
        [SerializeField] string _path;
        [SerializeField] string _bundleName;
        private T _asset;


        public Prefab() { }

        //-----------------------------------------------------------
        public string GUID => _guid;
        /// <summary> Path to the file (UnityEngine.Object). </summary>
        public string Path => _path;
        /// <summary> Bundle name. </summary>
        public string Bundle => _bundleName;

        //-----------------------------------------------------------


#if UNITY_EDITOR
        public Prefab(T asset) 
        { 
            _asset = asset;
        }

        //-----------------------------------------------------------
        /// <summary>Returns the asset's preview (available only in the editor!)</summary>
        public Texture2D Preview => AssetPreview.GetAssetPreview(Asset);

        public T Asset
        {
            get
            {
                if (_asset == null && !string.IsNullOrEmpty(_guid))
                {
                    _asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(_guid));
                }
                return _asset;
            }
        }

        public void Draw(string label)
        {
            _asset = EditorGUILayout.ObjectField(label, Asset, typeof(T), false) as T;
        }

        /// <summary>
        /// Refreshes all fields based on GUID. Returns true if changes were made.
        /// </summary>
        public bool Refresh()
        {
            string previousPath = _path;
            string previousBundleName = _bundleName;

            RefreshAssetProperties();

            return _path != previousPath || _bundleName != previousBundleName;
        }

        private void RefreshAssetProperties()
        {
            if (_asset == null) return;

            _path = AssetDatabase.GetAssetPath(_asset);
            _guid = AssetDatabase.AssetPathToGUID(_path);
            _bundleName = AssetImporter.GetAtPath(_path)?.assetBundleName ?? "";

            if (string.IsNullOrEmpty(_bundleName))
            {
                Debug.LogError($"Could not find bundle for {_path}");
            }
        }

        private void ClearProperties()
        {
            _guid = "";
            _path = "";
            _asset = null;
            _bundleName = "";
        }
        //-----------------------------------------------------------
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (_asset == null || !AssetDatabase.Contains(_asset))
            {
                ClearProperties();
                return;
            }
            RefreshAssetProperties();
#endif
        }

        public void OnAfterDeserialize() { }
    }
}
