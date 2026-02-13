using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IRCore.UnityHelpers
{
    [RequireComponent(typeof(Graphic))]
    public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Observable<Unit> OnClickAsObservable => _onClickSubject.AsObservable();
        public Observable<Unit> OnDownAsObservable => _onDownSubject.AsObservable();
        public Observable<Unit> OnUpAsObservable => _onUpSubject.AsObservable();
        public Observable<Unit> OnEnterAsObservable => _onEnterSubject.AsObservable();
        public Observable<Unit> OnExitAsObservable => _onExitSubject.AsObservable();
        public ReadOnlyReactiveProperty<bool> Interactable => _interactable;

        [Header("BaseSettings")]
        [SerializeField]
        protected bool m_initInteractable = true;

        #region Field
        protected readonly Subject<Unit> _onClickSubject = new();
        protected readonly Subject<Unit> _onDownSubject = new();
        protected readonly Subject<Unit> _onUpSubject = new();
        protected readonly Subject<Unit> _onEnterSubject = new();
        protected readonly Subject<Unit> _onExitSubject = new();
        protected readonly ReactiveProperty<bool> _interactable = new();
        #endregion

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void OnDestroy()
        {
            _onClickSubject.OnCompleted();
            _onClickSubject.Dispose();
            _onDownSubject.OnCompleted();
            _onDownSubject.Dispose();
            _onUpSubject.OnCompleted();
            _onUpSubject.Dispose();
            _onEnterSubject.OnCompleted();
            _onEnterSubject.Dispose();
            _onExitSubject.OnCompleted();
            _onExitSubject.Dispose();
            _interactable.Dispose();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!_interactable.CurrentValue) return;

            _onClickSubject.OnNext(Unit.Default);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!_interactable.CurrentValue) return;

            _onDownSubject.OnNext(Unit.Default);
        }
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!_interactable.CurrentValue) return;

            _onUpSubject.OnNext(Unit.Default);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!_interactable.CurrentValue) return;

            _onEnterSubject.OnNext(Unit.Default);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!_interactable.CurrentValue) return;

            _onExitSubject.OnNext(Unit.Default);
        }

        /// <summary>
        /// インタラクティブ状態を設定する
        /// </summary>
        /// <param name="interactable"></param>
        public virtual void SetInteractable(bool interactable)
        {
            _interactable.Value = interactable;
        }

        #region Initialize
        protected virtual void Initialize()
        {
            //  インタラクティブ設定を行う
            SetInteractable(m_initInteractable);
        }
        #endregion
    }
}