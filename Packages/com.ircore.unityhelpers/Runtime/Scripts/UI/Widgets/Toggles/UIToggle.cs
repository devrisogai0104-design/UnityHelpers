using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IRCore.UnityHelpers
{
    public class UIToggle : UIButton
    {
        public ReadOnlyReactiveProperty<bool> IsOn => _isOn;

        [Header("UIToggle - Settings")]
        [SerializeField]
        protected bool m_initIsOn = false;

        #region Field
        protected readonly ReactiveProperty<bool> _isOn = new();
        #endregion

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _isOn.OnCompleted();
            _isOn.Dispose();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            //  選択状態を変更する
            bool current = _isOn.Value;
            SetIsOn(!current);
        }

        /// <summary>
        /// 選択状態を設定する
        /// </summary>
        public virtual void SetIsOn(bool isOn)
        {
            _isOn.Value = isOn;
        }

        #region Initialize
        protected override void Initialize()
        {
            base.Initialize();

            //  選択状態を設定する
            SetIsOn(m_initIsOn);
        }
        #endregion
    }
}
