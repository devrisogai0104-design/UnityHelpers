#if UNITY_EDITOR
using IRCore.UnityHelpers.DebugManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace IRCore.UnityHelpers.Editor
{
    [InitializeOnLoad]
    public static class SceneAutoLoader
    {
        private const string ActiveSettingsPathKey = "IRCore_ActiveDebugSettingsPath";

        static SceneAutoLoader()
        {
            EditorApplication.update += UpdatePlayModeStartScene;
        }

        // WorkflowerƒNƒ‰ƒX‚â‘¼‚©‚çŒÄ‚Î‚ê‚é‘‹Œû
        public static void SetActiveSettings(DebugSettings settings)
        {
            if (settings == null)
            {
                EditorPrefs.DeleteKey(ActiveSettingsPathKey);
            }
            else
            {
                string path = AssetDatabase.GetAssetPath(settings);
                EditorPrefs.SetString(ActiveSettingsPathKey, path);
            }
            UpdatePlayModeStartScene();
        }

        private static void UpdatePlayModeStartScene()
        {
            if (EditorApplication.isPlaying) return;

            string path = EditorPrefs.GetString(ActiveSettingsPathKey, "");
            var debugSettings = AssetDatabase.LoadAssetAtPath<DebugSettings>(path);

            if (debugSettings == null)
            {
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            var sceneModule = debugSettings.GetModule<SceneDebugModule>();
            if (sceneModule != null && sceneModule.AutoLoadMasterScene && sceneModule.MasterSceneAsset != null)
            {
                EditorSceneManager.playModeStartScene = sceneModule.MasterSceneAsset;
            }
            else
            {
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}
#endif