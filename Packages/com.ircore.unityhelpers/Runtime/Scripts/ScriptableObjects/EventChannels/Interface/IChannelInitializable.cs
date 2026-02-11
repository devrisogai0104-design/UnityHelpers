namespace IRCore.UnityHelpers
{
    /// <summary>
    /// 初期化可能なチャンネルであることを示すインターフェース
    /// </summary>
    public interface IChannelInitializable
    {
        /// <summary>
        /// 初期化する
        /// </summary>
        void Initialize();
    }
}
