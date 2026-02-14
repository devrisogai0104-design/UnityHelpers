using IRCore.UnityHelpers.Networking;
using System;
using UnityEngine;
using R3;

namespace IRCore.UnityHelpers.Samples
{
    public class TcpSampleNetworkRunner : MonoBehaviour
    {
        private TcpApiClient _client;

        async void Start()
        {
            _client = new TcpApiClient();

            // 受信の購読
            _client.OnReceived
                .Subscribe(bytes => {
                    string msg = System.Text.Encoding.UTF8.GetString(bytes);
                    Debug.Log($"<color=cyan>[Received from Server]</color> {msg}");
                })
                .AddTo(this);

            // 1. 接続テスト
            Debug.Log("Connecting...");
            await _client.ConnectAsync("127.0.0.1", 8080, TimeSpan.FromSeconds(5));

            if (_client.IsConnected.CurrentValue)
            {
                // 2. 送信テスト
                byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello, TCP Server!");
                await _client.SendAsync(data);
                Debug.Log("Data Sent.");
            }
        }

        private void OnDestroy()
        {
            _client?.Dispose();
        }
    }
}
