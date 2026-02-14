using System.Collections.Generic;
using UnityEngine;

namespace IRCore.UnityHelpers.DebugManagement
{
    /// <summary>
    /// 開始シーンの判定モード
    /// </summary>
    public enum StartSceneMode
    {
        Default,    // 通常（タイトル画面など）
        Designated, // 特定の指定されたシーン
        Current     // 現在エディタで開いているシーン
    }

    [CreateAssetMenu(fileName = "SceneDebugModule", menuName = "IRCore/Debug/Modules/SceneDebugModule")]
    public class SceneDebugModule : DebugModule
    {
        [Header("Editor Play Mode Settings")]
        [Tooltip("再生ボタンを押した際、自動的にMasterシーンから開始するか")]
        public bool AutoLoadMasterScene = false;

        [Tooltip("自動ロードするMasterシーンのアセット（SceneAssetを指定）")]
#if UNITY_EDITOR
        public UnityEditor.SceneAsset MasterSceneAsset;
#endif

        [Header("Start Settings")]
        [Tooltip("デバッグ開始時にどのシーンから始めるか")]
        public StartSceneMode StartMode = StartSceneMode.Default;

        [Tooltip("StartModeがDesignatedの時にロードされるシーン名")]
        public string DesignatedSceneName;

        [Header("Required Scenes")]
        [Tooltip("シーン遷移に関わらず、常にロードされている必要があるシーンのリスト")]
        public List<string> RequiredSceneNames = new List<string>();
    }
}
