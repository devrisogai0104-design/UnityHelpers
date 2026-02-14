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

        // Workflowerクラスや他から呼ばれる窓口
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
                UnityEngine.Debug.Log("Masterロード失敗: 設定アセットが見つかりません");
                return;
            }

            var sceneModule = debugSettings.GetModule<SceneDebugModule>();
            if (sceneModule != null && sceneModule.AutoLoadMasterScene && sceneModule.MasterSceneAsset != null)
            {
                EditorSceneManager.playModeStartScene = sceneModule.MasterSceneAsset;
                UnityEngine.Debug.Log($"<color=yellow>[IRCore] Master Scene Set: {sceneModule.MasterSceneAsset.name}</color>");
            }
            else
            {
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}
#endif