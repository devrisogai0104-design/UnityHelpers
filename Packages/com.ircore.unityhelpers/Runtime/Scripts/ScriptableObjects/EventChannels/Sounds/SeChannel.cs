using R3;
using UnityEngine;

namespace IRCore.UnityHelpers
{
    [CreateAssetMenu(fileName = "NewSeChannel", menuName = "IRCore/Channels/Sounds/SeChannel")]
    public class SeChannel : ScriptableObject, IChannelInitializable
    {
        public Observable<AudioClip> OnPlay => _onPlay;
        public ReadOnlyReactiveProperty<float> Volume => _volume;
        public ReadOnlyReactiveProperty<bool> IsMute => _isMute;

        #region Inspector Fields
        [Header("Settings")]
        [SerializeField, Range(0f, 1f)]
        private float m_defaultVolume = 1.0f;
        [SerializeField]
        private bool m_defaultMute = false;
        #endregion

        #region Runtime Fields
        private readonly Subject<AudioClip> _onPlay = new();
        [SerializeField]
        private ReactiveProperty<float> _volume = new(1.0f);
        [SerializeField]
        private ReactiveProperty<bool> _isMute = new(false);
        #endregion

        public void Initialize()
        {
            _volume.Value = m_defaultVolume;
            _isMute.Value = m_defaultMute;
        }

        /// <summary>
        /// 音を再生する
        /// </summary>
        public void Play(AudioClip clip)
        {
            if (clip == null) return;
            // ミュート中はイベントを流さない
            if (_isMute.Value) return;

            _onPlay.OnNext(clip);
        }

        /// <summary>
        /// 音量を設定する
        /// </summary>
        public void SetVolume(float volume)
        {
            _volume.Value = Mathf.Clamp01(volume);
            m_defaultVolume = _volume.Value;
        }

        /// <summary>
        /// ミュート状態を設定する
        /// </summary>
        public void SetMute(bool mute)
        {
            _isMute.Value = mute;
            m_defaultMute = _isMute.Value;
        }

        private void OnDisable()
        {
            _onPlay.OnCompleted();
            _onPlay.Dispose();
            _volume.Dispose();
            _isMute.Dispose();
        }
    }
}
