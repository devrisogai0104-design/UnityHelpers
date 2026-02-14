using System.Collections.Generic;
using UnityEngine;

namespace IRCore.UnityHelpers.Scene
{
    [System.Serializable]
    public class SceneRequirement
    {
        public string TargetSceneName;
        public bool NeedsUserData;   // APIロードが必要か
        public bool NeedsMasterData; // マスタ読込が必要か
    }

    [CreateAssetMenu(fileName = "SceneModule", menuName = "IRCore/Scene/SceneModule")]
    public class SceneModule : ScriptableObject
    {
        public List<string> EssentialScenes = new List<string>(); // Masterなど
        public float FadeOutDuration = 0.5f;
        public float FadeInDuration = 0.5f;

        // シーン名と要件を紐付けるリスト
        public List<SceneRequirement> Requirements = new List<SceneRequirement>();

        public SceneRequirement GetRequirement(string sceneName)
            => Requirements.Find(r => r.TargetSceneName == sceneName);
    }
}
