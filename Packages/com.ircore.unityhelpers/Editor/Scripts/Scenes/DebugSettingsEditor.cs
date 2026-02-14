#if UNITY_EDITOR
using IRCore.UnityHelpers.Debug;
using UnityEditor;
using UnityEngine;

namespace IRCore.UnityHelpers.Editor
{
    [CustomEditor(typeof(DebugSettings))]
    public class DebugSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // 現在のアセットがアクティブかどうか判定
            string activePath = EditorPrefs.GetString("IRCore_ActiveDebugSettingsPath", "");
            string currentPath = AssetDatabase.GetAssetPath(target);
            bool isActive = activePath == currentPath;

            // アクティブなら目立つバッジを表示
            if (isActive)
            {
                GUI.backgroundColor = Color.cyan;
                GUILayout.BeginVertical("HelpBox");
                EditorGUILayout.LabelField("● ACTIVE DEBUG SETTINGS", EditorStyles.boldLabel);
                GUILayout.EndVertical();
                GUI.backgroundColor = Color.white;
            }
            else
            {
                if (GUILayout.Button("Set as Active Debug Settings", GUILayout.Height(30)))
                {
                    SceneAutoLoader.SetActiveSettings((DebugSettings)target);
                }
            }

            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
}
#endif