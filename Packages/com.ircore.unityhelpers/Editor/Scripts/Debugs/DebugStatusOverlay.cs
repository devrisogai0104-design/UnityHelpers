#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using IRCore.UnityHelpers.DebugManagement;

namespace IRCore.UnityHelpers.Editor
{
    [InitializeOnLoad]
    public static class DebugStatusOverlay
    {
        private const string ActiveSettingsPathKey = "IRCore_ActiveDebugSettingsPath";

        static DebugStatusOverlay()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            // 1. 保存されたパスから設定アセットを取得
            string path = EditorPrefs.GetString(ActiveSettingsPathKey, "");
            if (string.IsNullOrEmpty(path)) return;

            var debugSettings = AssetDatabase.LoadAssetAtPath<DebugSettings>(path);
            if (debugSettings == null) return;

            // 2. DebugSettings直下の表示フラグを確認（ここを修正）
            if (!debugSettings.ShowOverlay) return;

            // --- 描画開始 ---
            Handles.BeginGUI();

            // スタイルの設定
            var boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.8f)); // 視認性向上のため暗めに

            var titleStyle = new GUIStyle(EditorStyles.boldLabel);
            titleStyle.fontSize = 13;
            titleStyle.normal.textColor = Color.cyan;

            var statusStyle = new GUIStyle(EditorStyles.label);
            statusStyle.fontSize = 11;
            statusStyle.normal.textColor = Color.white;

            var linkStyle = new GUIStyle(EditorStyles.linkLabel);
            linkStyle.fontSize = 11;
            linkStyle.alignment = TextAnchor.MiddleRight; // 右寄せ

            // エリアの配置（右上に固定）
            float width = 240;
            float height = 75;
            float margin = 10;
            Rect areaRect = new Rect(sceneView.position.width - width - margin, margin, width, height);

            GUILayout.BeginArea(areaRect, boxStyle);
            GUILayout.BeginVertical();
            GUILayout.Space(5);

            // 設定名
            EditorGUILayout.LabelField($"● {debugSettings.name}", titleStyle);

            // Masterシーン強制ロードの状態
            bool isMasterForced = EditorSceneManager.playModeStartScene != null;
            string masterInfo = isMasterForced ? "Master-First: ON" : "Current-Scene: ON";

            GUI.color = isMasterForced ? Color.yellow : Color.gray;
            EditorGUILayout.LabelField(masterInfo, statusStyle);
            GUI.color = Color.white;

            GUILayout.FlexibleSpace();

            // プロジェクトウィンドウへのリンク
            if (GUILayout.Button("→ Locate Asset in Project", linkStyle))
            {
                EditorGUIUtility.PingObject(debugSettings);
                Selection.activeObject = debugSettings;
            }

            GUILayout.Space(5);
            GUILayout.EndVertical();
            GUILayout.EndArea();

            Handles.EndGUI();
        }

        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i) pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
#endif