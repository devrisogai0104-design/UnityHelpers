using IRCore.UnityHelpers.Debug;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IRCore.UnityHelpers.Scene
{
    public abstract class SceneBootstrapperBase
    {
        protected readonly IDebugManager DebugManager;
        protected readonly SceneModule SceneModule;
        protected readonly IFader Fader;

        protected SceneBootstrapperBase(
            IDebugManager debugManager,
            SceneModule sceneModule,
            IFader fader)
        {
            DebugManager = debugManager;
            SceneModule = sceneModule;
            Fader = fader;
        }

        // 起動時専用のメソッドを Base に追加
        public virtual async Task InitializeFirstScene()
        {
            // 1. まず「Masterシーン（EssentialScenesの最初など）」をSingleでロード
            // これにより、中途半端に開いているシーンを掃除し、DIコンテナを確立させる
            if (SceneModule.EssentialScenes.Count > 0)
            {
                string master = SceneModule.EssentialScenes[0];
                await SceneManager.LoadSceneAsync(master, LoadSceneMode.Single);
            }

            // 2. その後、通常の遷移ロジック（デバッグ判定含む）を走らせる
            // 引数 null を渡せば、DetermineTargetScene がデバッグ設定を見てくれる
            await NavigateToScene(null);
        }

        public virtual async Task NavigateToScene(string targetSceneName = null)
        {
            // 1. フェードアウト開始
            Task fadeOutTask = Fader.FadeOut(SceneModule.FadeOutDuration);

            // 2. ロードシーケンス（演出と並列実行）
            Task loadProcessTask = ExecuteLoadSequence(targetSceneName);

            await Task.WhenAll(fadeOutTask, loadProcessTask);

            // 3. ロード完了後の追加処理（ここでAPIロードなどを差し込む）
            await OnBeforeFadeIn();

            // 4. フェードイン
            await Fader.FadeIn(SceneModule.FadeInDuration);
        }

        // --- Hook Methods (継承先でロジックを挟み込む場所) ---

        protected virtual Task OnBeforeFadeIn() => Task.CompletedTask;

        // --- Core Logic (必要に応じて override 可能) ---
        protected virtual async Task ExecuteLoadSequence(string targetSceneName)
        {
            // A. 必須シーンロード
            await LoadScenesIncremental(SceneModule.EssentialScenes);

            // B. デバッグ用シーンロード
            if (DebugManager.IsActive<SceneDebugModule>())
            {
                var debugModule = DebugManager.GetSettings<SceneDebugModule>();
                await LoadScenesIncremental(debugModule.RequiredSceneNames);
            }

            // C. メインシーンロード
            string finalSceneName = DetermineTargetScene(targetSceneName);
            await LoadMainSceneAsync(finalSceneName);
        }


        protected virtual string DetermineTargetScene(string manualTarget)
        {
            if (DebugManager.IsActive<SceneDebugModule>())
            {
                var debug = DebugManager.GetSettings<SceneDebugModule>();
                switch (debug.StartMode)
                {
                    case StartSceneMode.Designated:
                        if (!string.IsNullOrEmpty(debug.DesignatedSceneName))
                            return debug.DesignatedSceneName;
                        break;
                    case StartSceneMode.Current:
#if UNITY_EDITOR
                        return SceneManager.GetActiveScene().name;
#endif
                        break;
                }
            }
            return manualTarget ?? "TitleScene";
        }

        protected async Task LoadScenesIncremental(List<string> sceneNames)
        {
            if (sceneNames == null) return;
            foreach (var name in sceneNames)
            {
                if (string.IsNullOrEmpty(name)) continue;
                if (!SceneManager.GetSceneByName(name).isLoaded)
                {
                    var op = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
                    while (!op.isDone) await Task.Yield();
                }
            }
        }

        protected virtual async Task LoadMainSceneAsync(string sceneName)
        {
            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                Fader.SetProgress(op.progress);
                await Task.Yield();
            }

            Fader.SetProgress(1.0f);
            op.allowSceneActivation = true;
            while (!op.isDone) await Task.Yield();
        }
    }
}