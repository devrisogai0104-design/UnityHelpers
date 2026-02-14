using UnityEngine;

namespace IRCore.UnityHelpers.Debug
{
    public interface IDebugManager
    {
        /// <summary>
        /// マスターフラグとモジュールフラグの両方が有効か判定する
        /// </summary>
        bool IsActive<T>() where T : DebugModule;
        /// <summary>
        /// 設定値そのものを取得したい場合に使用（例：ログの色など）
        /// </summary>
        T GetSettings<T>() where T : DebugModule;
    }
}
