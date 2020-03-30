using UnityEditor;
using System.Collections.Generic;

namespace TeamZero.Core.Unity.UnityEditorUtility
{
    public class BuildSceneProcessor : UnityEditor.AssetModificationProcessor
    {
        private const string DIALOG_TITLE = "Add to Build Settings?";
        private const string DIALOG_MSG = "Add to build settings for inclusion in future builds?";
        private const string DIALOG_OK = "Yes";
        private const string DIALOG_NO = "Not now";

        public static void OnWillCreateAsset(string path)
        {
            if (path.EndsWith(".unity.meta"))
                path = path.Substring(0, path.Length - 5);

            ProcessAssetsForScenes(new string[] {path});
        }

        public static string[] OnWillSaveAssets(string[] paths)
        {
            return ProcessAssetsForScenes(paths);
        }

        private static string[] ProcessAssetsForScenes(string[] paths)
        {
            string scenePath = string.Empty;
            foreach (string path in paths)
            {
                if (path.Contains(".unity"))
                    scenePath = path;
            }

            if (!string.IsNullOrEmpty(scenePath))
                AddSceneToBuildSettings(scenePath);

            // unity only saves the paths that you return here, so we always pass through everything we received.
            return paths;
        }

        
        private static readonly List<string> _ignorePaths = new List<string>();
        private static void AddSceneToBuildSettings(string scenePath)
        {
            var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (scene.path == scenePath)
                    return;
            }

            if (EditorUtility.DisplayDialog(DIALOG_TITLE, DIALOG_MSG, DIALOG_OK, DIALOG_NO))
            {
                EditorBuildSettingsScene newScene = new EditorBuildSettingsScene();
                newScene.path = scenePath;
                newScene.enabled = true;

                scenes.Add(newScene);
                EditorBuildSettings.scenes = scenes.ToArray();
            }
            else
            {
                _ignorePaths.Add(scenePath);
            }
        }
    }
}