using UnityEngine;

namespace IRCore.UnityHelpers.DebugManagement
{
    public class DebugManager : IDebugManager
    {
        protected readonly DebugSettings _settings;

        public DebugManager(DebugSettings setting)
        {
            _settings = setting;
        }

        public virtual bool IsActive<T>() where T : DebugModule
        {
            // 1. 全体のデバッグモードがオフなら即座に拒否
            if (!_settings.IsDebugMode) return false;

            // 2. モジュールの取得
            var module = _settings.GetModule<T>();

            // 3. 未登録なら「無効」として扱う（エラーにはしない）
            if (module == null) return false;

            // 4. モジュール個別の有効フラグを返す
            return module.IsEnabled;
        }

        public virtual T GetSettings<T>() where T : DebugModule
        {
            var module = _settings.GetModule<T>();
            if (module != null) return module;

            // 見つからない場合は一時的なデフォルト値を生成して返す（Null参照防止）
            return ScriptableObject.CreateInstance<T>();
        }
    }
}
