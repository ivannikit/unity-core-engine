#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using TeamZero.Core.Logging;
using Object = UnityEngine.Object;

namespace TeamZero.Core.Unity
{
    public static class AssetDatabaseHelper
    {
        public static string GetSystemPatch(string assetPath, TZLogger logger = null)
        {
            if (!string.IsNullOrEmpty(assetPath))
            {
                string assetsFolderPath = Path.GetDirectoryName(Application.dataPath);
                return Path.Combine(assetsFolderPath, assetPath); 
            }
            else
            {
                logger?.Throw(new NullReferenceException());
                return null;
            }
        }
        
        /// <summary>
        /// Create folder by asset path if it's not exists
        /// </summary>
        public static void CreateFolder(string assetPath, TZLogger logger = null)
        {
            string systemPatch = GetSystemPatch(assetPath, logger);
            Directory.CreateDirectory(systemPatch);
        }
        
        /// <summary>
        /// Delete empty folder by asset path if it's not exists
        /// </summary>
        public static void DeleteEmptyFoldersFromPatch(string assetPath, TZLogger logger = null)
        {
            string systemPatch = GetSystemPatch(assetPath, logger);
            DirectoryInfo dir = new DirectoryInfo(systemPatch);
            while (true)
            {
                if(dir == null)
                    break;
                
                var files = dir.GetFiles();
                if (files == null || files.Length == 0)
                {
                    Directory.Delete(dir.FullName);
                    dir = dir.Parent;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Returns asset importer for asset object 
        /// </summary>
        public static T GetAssetImporter<T>(Object assetObject) where T : AssetImporter
        {
            string path = AssetDatabase.GetAssetPath(assetObject);
            if (string.IsNullOrEmpty(path))
                return null;

            return AssetImporter.GetAtPath(path) as T;
        }

        public static bool TryCreateDirectory(string assetFolderPath, TZLogger logger = null)
        {
            bool result = false;

            string fullFolderPath = Path.Combine(Application.dataPath, assetFolderPath);
            if (!Directory.Exists(fullFolderPath))
            {
                logger?.Info($"'{assetFolderPath}' not exists, create it");
                Directory.CreateDirectory(fullFolderPath);
                AssetDatabase.Refresh();
                result = true;
            }
            else
            {
                logger?.Error($"Directory '{assetFolderPath}' already exist");
            }

            return result;
        }

        public static T CreateAsset<T>(string assetFolderPath, string fileName, TZLogger logger = null) where T : Object, new()
        {
            string assetFilePath = Path.Combine(assetFolderPath, fileName);
            assetFilePath = AssetDatabase.GenerateUniqueAssetPath(assetFilePath);

            TryCreateDirectory(assetFolderPath, logger);
            T inst = new T();
            AssetDatabase.CreateAsset(inst, assetFilePath);
            SaveAndRefresh();

            logger?.Info($"An instance of {typeof(T).Name} was created in '{assetFilePath}'.");
            return inst;
        }
        
        public static bool DeleteAsset(string assetPath)
        {
            bool result = AssetDatabase.DeleteAsset(assetPath);
            DeleteEmptyFoldersFromPatch(assetPath);
            SaveAndRefresh();

            return result;
        }

        public static void SaveAndRefresh()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void ShowInProjectView(Object asset)
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        public static class Selected
        {
            [System.Flags]
            public enum GetDirectoryOptions
            {
                Default = None,
                None = 0,
                ConvertSelectedObjectToDirectory = 2,
            }

            public static string GetDirectoryAssetPath(string defaultValue = "Asset/", GetDirectoryOptions options = GetDirectoryOptions.Default)
            {
                string assetPath = defaultValue;

                Object selected = Selection.activeObject;
                if (selected != null)
                {
                    string selectedPath = AssetDatabase.GetAssetPath(selected);
                    if (Directory.Exists(selectedPath))
                    {
                        assetPath = selectedPath;
                    }
                    else
                    {
                        bool convertObject = (options & GetDirectoryOptions.ConvertSelectedObjectToDirectory) != 0;
                        if (convertObject)
                            assetPath = Path.GetDirectoryName(selectedPath);
                    }

                }

                return assetPath;
            }
        }
    }
}
#endif