using System;
using System.Collections.Generic;
using UnityEngine;

namespace IRCore.UnityHelpers.DebugManagement
{
    [CreateAssetMenu(fileName = "DebugSetting", menuName = "IRCore/Debug/DebugSetting")]
    public class DebugSettings : ScriptableObject
    {
        public bool IsDebugMode => _isDebugMode;

        [Header("デバッグモードのON/OFF")]
        [SerializeField] 
        private bool _isDebugMode = true;

        [Header("Global Editor Settings")]
        [Tooltip("シーンビューのデバッグオーバーレイを表示するか")]
        public bool ShowOverlay = true;

        [Header("モジュール")]
        [SerializeField]
        private List<DebugModuleBase> _modules = new();

        // 型をキーにしたキャッシュ
        private readonly Dictionary<Type, DebugModuleBase> _cache = new();

        // 実行順に依存しないための初期化メソッド
        private void EnsureCache()
        {
            if (_cache.Count > 0 || _modules.Count == 0) return;

            foreach (var module in _modules)
            {
                if (module != null)
                {
                    _cache[module.GetType()] = module;
                }
            }
        }

        public T GetModule<T>() where T : DebugModuleBase
        {
            EnsureCache();

            if (_cache.TryGetValue(typeof(T), out var module))
            {
                return module as T;
            }
            return null;
        }

        // インスペクターで値をいじった時にキャッシュをクリアする（Editor用）
        private void OnValidate() => _cache.Clear();

        /// <summary>
        /// リストの中身の差し替えを行う
        /// </summary>
        /// <param name="newModules"></param>
        public void ClearAndReassignModules(System.Collections.Generic.List<DebugModuleBase> newModules)
        {
            // 既存のリスト（例：private List<DebugModuleBase> _modules）をクリア
            this._modules.Clear();

            // 新しくコピーされたフォルダ内のモジュールを追加
            foreach (var m in newModules)
            {
                this._modules.Add(m);
            }
        }
    }
}
