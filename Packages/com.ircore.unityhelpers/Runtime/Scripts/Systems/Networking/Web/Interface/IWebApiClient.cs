using Cysharp.Threading.Tasks;
using System.Threading;

namespace IRCore.UnityHelpers.Networking
{
    public interface IWebApiClient
    {
        /// <summary>
        /// GET: データ取得
        /// </summary>
        UniTask<string> GetAsync(string url, int timeoutSeconds = 10, CancellationToken ct = default);

        /// <summary>
        /// POST: 新規作成 (JSON送信)
        /// </summary>
        UniTask<string> PostAsync(string url, string json, int timeoutSeconds = 10, CancellationToken ct = default);

        /// <summary>
        /// PUT: 更新 (JSON送信)
        /// </summary>
        UniTask<string> PutAsync(string url, string json, int timeoutSeconds = 10, CancellationToken ct = default);

        /// <summary>
        /// DELETE: 削除
        /// </summary>
        UniTask<string> DeleteAsync(string url, int timeoutSeconds = 10, CancellationToken ct = default);
    }
}
