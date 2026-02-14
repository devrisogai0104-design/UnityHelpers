using R3;
using UnityEngine;

namespace IRCore.UnityHelpers
{
    [RequireComponent(typeof(UIButton))]
    public class UIButtonClickAudio : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] 
        private AudioClip m_clickSe;
        [SerializeField]
        private SeChannel m_seChannel;

        protected UIButton _button;

        protected virtual void Awake()
        {
            _button = GetComponent<UIButton>();

            if (m_clickSe == null)
                Debug.LogWarning($"[IRCore] AudioClip is missing on {gameObject.name}.", this);

            if (m_seChannel == null)
                Debug.LogWarning($"[IRCore] SeChannel is missing on {gameObject.name}.", this);

            Bind();
        }

        protected virtual void Bind()
        {
            _button.OnClick
                .Select(_ => m_clickSe)
                .Where(clip => clip != null)
                .Subscribe(clip => PlayClickSe(clip))
                .AddTo(this);
        }

        protected virtual void PlayClickSe(AudioClip clip)
        {
            m_seChannel?.Play(clip);
        }
    }
}