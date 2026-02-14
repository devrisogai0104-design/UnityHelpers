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
                EditorUtility.DisplayDialog("IRCore Error",
                    $"Template folder not found at: {TemplateFolderPath}\n" +
                    "Please make sure the path is correct.", "OK");
                return;
            }

            // 2. 保存先の名前と場所をユーザーに決めさせる
            // デフォルト名を現在のテンプレートフォルダ名 + _Copy に設定
            string defaultName = Path.GetFileName(TemplateFolderPath) + "_User";
            string destinationPath = EditorUtility.SaveFilePanelInProject(
                "Copy Template Folder",
                defaultName,
                "",
                "Select where to save your personal debug settings folder");

            if (string.IsNullOrEmpty(destinationPath)) return;

            // 3. フォルダごとコピー
            if (AssetDatabase.CopyAsset(TemplateFolderPath, destinationPath))
            {
                AssetDatabase.Refresh();

                // 4. コピー先のフォルダ内にある DebugSettings を探してアクティブにする
                string[] guids = AssetDatabase.FindAssets("t:DebugSettings", new[] { destinationPath });

                if (guids.Length > 0)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    var newSettings = AssetDatabase.LoadAssetAtPath<DebugSettings>(assetPath);

                    if (newSettings != null)
                    {
                        SceneAutoLoader.SetActiveSettings(newSettings);

                        EditorGUIUtility.PingObject(newSettings);
                        Selection.activeObject = newSettings;

                        UnityEngine.Debug.Log($"<color=cyan>[IRCore] Template Copied & Activated: {newSettings.name}</color>");
                    }
                }
            }
            else
            {
                UnityEngine.Debug.LogError($"[IRCore] Failed to copy template from {TemplateFolderPath}");
            }
        }

        /*
        // --- その他の既存メソッド (SetSelectedActive など) ---
        [MenuItem("IRCore/Debug/Set Selected as Active")]
        public static void SetSelectedActive()
        {
            var settings = Selection.activeObject as DebugSettings;
            if (settings != null)
            {
                SceneAutoLoader.SetActiveSettings(settings);
                UnityEngine.Debug.Log($"<color=cyan>[IRCore] Activated: {settings.name}</color>");
            }
        }
        */
    }
}
#endif