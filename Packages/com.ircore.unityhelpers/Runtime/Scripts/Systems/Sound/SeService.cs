using R3;
using System;
using UnityEngine;

namespace IRCore.UnityHelpers
{
    public class SeService : IDisposable
    {
        public Observable<AudioClip> OnPlayedAsObservable => _onPlayedSubject.AsObservable();
        public ReadOnlyReactiveProperty<float> Volume => _volume;
        public ReadOnlyReactiveProperty<bool> IsMute => _isMute;

        #region
        private Subject<AudioClip> _onPlayedSubject = new Subject<AudioClip>();
        private readonly ReactiveProperty<float> _volume = new(1.0f);
        private readonly ReactiveProperty<bool> _isMute = new(false);
        #endregion

        public void Dispose()
        {
            _onPlayedSubject?.OnCompleted();
            _onPlayedSubject?.Dispose();
            _volume?.Dispose();
            _isMute?.Dispose();
        }

        /// <summary>
        /// çƒê∂Ç∑ÇÈ
        /// </summary>
        /// <param name="clip"></param>
        public void Play(AudioClip clip)
        {
            if (clip == null) return;
            if (_isMute.Value) return;

            _onPlayedSubject.OnNext(clip);
        }

        public void SetVolume(float value) => _volume.Value = Mathf.Clamp01(value);
        public void SetMute(bool mute) => _isMute.Value = mute;
    }
}
