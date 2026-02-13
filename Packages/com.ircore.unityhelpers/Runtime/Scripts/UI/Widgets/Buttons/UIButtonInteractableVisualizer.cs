using R3;
using UnityEngine;
using UnityEngine.UI;

namespace IRCore.UnityHelpers
{
    [RequireComponent(typeof(UIButton))]
    public class UIButtonInteractableVisualizer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Graphic m_targetGraphic;

        [Header("Settings")]
        [SerializeField, ColorUsage(true, false)]
        private Color m_interactableColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        [SerializeField, ColorUsage(true, false)]
        private Color m_nonInteractableColor = new Color(1.0f, 1.0f, 1.0f, 0.3f);

        protected UIButton _button;

        protected virtual void Awake()
        {
            _button = GetComponent<UIButton>();

            Bind();
        }

        protected virtual void UpdateVisual(bool interactable)
        {
            if (m_targetGraphic == null) return;

            // ŽO€‰‰ŽZŽq‚ÅF‚ðØ‚è‘Ö‚¦
            m_targetGraphic.color = interactable ? m_interactableColor : m_nonInteractableColor;
        }

        protected virtual void Bind()
        {
            _button.Interactable
                .Subscribe(interactable => UpdateVisual(interactable))
                .AddTo(this);
        }
    }
}
