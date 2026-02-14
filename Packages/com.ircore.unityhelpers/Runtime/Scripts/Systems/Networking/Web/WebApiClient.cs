using Cysharp.Threading.Tasks;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace IRCore.UnityHelpers.Networking
{
    public class WebApiClient : IWebApiClient
    {
        public async UniTask<string> GetAsync(string url, int timeoutSeconds = 10, CancellationToken ct = default)
        {
            return await SendRequestAsync(UnityWebRequest.Get(url), timeoutSeconds, ct);
        }

        public async UniTask<string> PostAsync(string url, string json, int timeoutSeconds = 10, CancellationToken ct = default)
        {
            var request = new UnityWebRequest(url, "POST");
            SetupJsonRequest(request, json);
            return await SendRequestAsync(request, timeoutSeconds, ct);
        }

        public async UniTask<string> PutAsync(string url, string json, int timeoutSeconds = 10, CancellationToken ct = default)
        {
            var request = new UnityWebRequest(url, "PUT");
            SetupJsonRequest(request, json);
            return await SendRequestAsync(request, timeoutSeconds, ct);
        }

        public async UniTask<string> DeleteAsync(string url, int timeoutSeconds = 10, CancellationToken ct = default)
        {
            return await SendRequestAsync(UnityWebRequest.Delete(url), timeoutSeconds, ct);
        }

        /// <summary>
        /// 共通のリクエスト送信処理
        /// </summary>
        private async UniTask<string> SendRequestAsync(UnityWebRequest request, int timeout, CancellationToken ct)
        {
            using (request)
            {
                request.timeout = timeout;
                try
                {
                    await request.SendWebRequest().WithCancellation(ct);

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"[API Error] {request.method} {request.url} : {request.error}");
                        throw new System.Exception(request.error);
                    }

                    return request.downloadHandler?.text;
                }
                catch (System.OperationCanceledException)
                {
                    Debug.LogWarning($"[API Canceled] {request.method} {request.url}");
                    throw;
                }
            }
        }

        /// <summary>
        /// JSON送信用設定の共通化
        /// </summary>
        private void SetupJsonRequest(UnityWebRequest request, string json)
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
        }
    }
}
