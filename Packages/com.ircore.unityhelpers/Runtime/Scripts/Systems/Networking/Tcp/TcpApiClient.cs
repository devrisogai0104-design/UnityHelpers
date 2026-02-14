using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace IRCore.UnityHelpers.Networking
{
    public class TcpApiClient : IDisposable
    {
        public Observable<byte[]> OnReceived => _onReceived;
        public ReadOnlyReactiveProperty<bool> IsConnected => _isConnected;

        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private CancellationTokenSource _cts;

        #region R3
        private readonly Subject<byte[]> _onReceived = new();
        private readonly ReactiveProperty<bool> _isConnected = new(false);
        #endregion

        public async UniTask ConnectAsync(string ipAddress, int port, TimeSpan timeout, CancellationToken ct = default)
        {
            if (_isConnected.Value) return;

            _cts?.Cancel();
            _cts?.Dispose();

            try
            {
                _tcpClient = new TcpClient();

                // UniTaskのTimeout機能を使って、指定時間内に繋がらなければ例外を投げる
                await _tcpClient.ConnectAsync(ipAddress, port)
                    .AsUniTask()
                    .Timeout(timeout)
                    .AttachExternalCancellation(ct);

                _stream = _tcpClient.GetStream();
                _isConnected.Value = true;
                _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

                // 受信ループを開始（スレッドをブロックしないようにUniTaskで回す）
                ReceiveLoop(_cts.Token).Forget();
                Debug.Log($"[TcpClient] Connected to {ipAddress}:{port}");
            }
            catch (OperationCanceledException)
            {
                // 外部からのキャンセル（ct）による中断
                Debug.LogWarning("[TcpClient] Connection canceled by token.");
                Disconnect();
            }
            catch (TimeoutException)
            {
                Debug.LogError($"[TcpClient] Connection Timeout: {ipAddress}:{port} に時間内に繋がリませんでした。");
                Disconnect();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TcpClient] Connection Error: {ex.Message}");
                Disconnect();
            }
        }

        private async UniTaskVoid ReceiveLoop(CancellationToken ct)
        {
            var buffer = new byte[4096];
            try
            {
                while (_tcpClient is { Connected: true } && !ct.IsCancellationRequested)
                {
                    // データの到着を待機
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length, ct);
                    if (bytesRead <= 0) break; // 切断された

                    // 読み取った分だけコピーして通知
                    var receivedData = new byte[bytesRead];
                    Array.Copy(buffer, receivedData, bytesRead);
                    _onReceived.OnNext(receivedData);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Debug.LogError($"[TcpClient] Receive Error: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        public async UniTask SendAsync(byte[] data)
        {
            if (!_isConnected.Value || _stream == null) return;
            await _stream.WriteAsync(data, 0, data.Length);
        }

        public void Disconnect()
        {
            // 通信のリソース（ソケット、ストリーム、CancellationToken）だけを閉じる
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _stream?.Close();
            _stream = null;

            _tcpClient?.Close();
            _tcpClient = null;

            _isConnected.Value = false;
            Debug.Log("[TcpClient] Disconnected.");
        }

        public void Dispose()
        {
            // クラスが破棄されるとき（Singletonの寿命が尽きるとき）だけ呼ぶ
            Disconnect();

            // Subject類はここで初めてDisposeする
            _onReceived.OnCompleted();
            _onReceived.Dispose();
            _isConnected.Dispose();
        }
    }
}
