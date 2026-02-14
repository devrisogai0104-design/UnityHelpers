#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using IRCore.UnityHelpers.Debug;

namespace IRCore.UnityHelpers.Editor
{
    [InitializeOnLoad]
    public static class SceneAutoLoader
    {
        private const string ActiveSettingsPathKey = "IRCore_ActiveDebugSettingsPath";

        static SceneAutoLoader()
        {
            // 初回読み込み時とアップデート時に実行
            EditorApplication.update += UpdatePlayModeStartScene;
        }

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

            // 1. パスを取得。デフォルトは空文字（false扱い）
            string path = EditorPrefs.GetString(ActiveSettingsPathKey, "");

            // パスが空、またはアセットがロードできない場合は標準挙動（null）にする
            if (string.IsNullOrEmpty(path))
            {
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            var debugSettings = AssetDatabase.LoadAssetAtPath<DebugSettings>(path);
            if (debugSettings == null)
            {
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            // 2. DebugSettings 内の SceneDebugModule を取得
            var sceneModule = debugSettings.GetModule<SceneDebugModule>();

            // 3. 全ての条件（フラグがONかつアセットが指定されている）を満たさない限り、常にfalse（標準挙動）
            if (sceneModule != null &&
                sceneModule.AutoLoadMasterScene && // ここがデフォルトでfalse扱い
                sceneModule.MasterSceneAsset != null)
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