using UnityEngine;
using R3;

namespace IRCore.UnityHelpers.Samples
{
    public class SampleUIController : MonoBehaviour
    {
        [SerializeField]
        private UIButton m_button;
        [SerializeField]
        private UIToggle m_toggle;

        public void Start()
        {
            m_button.OnClickAsObservable
                .Subscribe(_ => Debug.Log($"Button Click!"))
                .AddTo(this);

            m_toggle.IsOn
                .Subscribe(isOn => Debug.Log($"Toggle Click! => {isOn}"))
                .AddTo(this);
        }
    }
}
