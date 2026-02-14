#if UNITY_EDITOR
using IRCore.UnityHelpers.DebugManagement;
using System.IO;
using UnityEditor;

namespace IRCore.UnityHelpers.Editor
{
    public static class DebugSettingsWorkflower
    {
        // 【設定】コピー元となるテンプレートフォルダのパスを指定してください
        private const string TemplateFolderPath = "Packages\\com.ircore.unityhelpers\\Runtime\\Settings\\Debug";

        [MenuItem("IRCore/Debug/Copy Template and Activate")]
        public static void CopyTemplateAndActivate()
        {
            // 1. テンプレートフォルダの存在確認
            if (!AssetDatabase.IsValidFolder(TemplateFolderPath))
            {
                EditorUtility.DisplayDialog("Error", "Template folder not found.", "OK");
                return;
            }

            // 2. 保存先を決定
            string destinationPath = EditorUtility.SaveFilePanelInProject(
                "Copy Template Folder", "NewDebugSettings", "", "Select save location");

            if (string.IsNullOrEmpty(destinationPath)) return;

            // 3. フォルダごとコピー
            if (AssetDatabase.CopyAsset(TemplateFolderPath, destinationPath))
            {
                AssetDatabase.Refresh();

                // 4. 新しいフォルダ内の「DebugSettings」と「全Module」を取得
                string[] allAssetPaths = AssetDatabase.FindAssets("t:UnityEngine.Object", new[] { destinationPath });

                DebugSettings newSettings = null;
                var newModules = new System.Collections.Generic.List<DebugModuleBase>();

                foreach (var guid in allAssetPaths)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                    if (obj is DebugSettings settings) newSettings = settings;
                    if (obj is DebugModuleBase module) newModules.Add(module);
                }

                // 5. 【重要】参照の付け替え
                if (newSettings != null)
                {
                    // 一旦リストをクリアして、同じフォルダ内にコピーされたモジュールを登録し直す
                    // ※DebugSettingsにModulesリストへアクセスできるメソッドや公開変数がある前提です
                    newSettings.ClearAndReassignModules(newModules);

                    EditorUtility.SetDirty(newSettings);
                    AssetDatabase.SaveAssets();

                    // アクティブにする
                    SceneAutoLoader.SetActiveSettings(newSettings);
                    EditorGUIUtility.PingObject(newSettings);
                    Selection.activeObject = newSettings;

                    UnityEngine.Debug.Log($"<color=cyan>[IRCore] Reassigned {newModules.Count} modules to new settings.</color>");
                }
            }
        }
    }
}
#endif