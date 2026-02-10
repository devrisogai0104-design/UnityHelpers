using R3;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Observable<Unit> OnClickAsObservable => _onClickSubject.AsObservable();
    public Observable<Unit> OnDownAsObservable => _onDownSubject.AsObservable();
    public Observable<Unit> OnUpAsObservable => _onUpSubject.AsObservable();
    public Observable<Unit> OnEnterAsObservable => _onEnterSubject.AsObservable();
    public Observable<Unit> OnExitAsObservable => _onExitSubject.AsObservable();
    public ReadOnlyReactiveProperty<bool> Interactable => _interactable;

    [Header("Settings")]
    [SerializeField]
    private bool m_initInteractable;

    #region Field
    private Subject<Unit> _onClickSubject = new();
    private Subject<Unit> _onDownSubject = new();
    private Subject<Unit> _onUpSubject = new();
    private Subject<Unit> _onEnterSubject = new();
    private Subject<Unit> _onExitSubject = new();
    private ReactiveProperty<bool> _interactable = new();
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_interactable.CurrentValue) return;

        _onClickSubject?.OnNext(Unit.Default);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_interactable.CurrentValue) return;

        _onDownSubject?.OnNext(Unit.Default);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_interactable.CurrentValue) return;

        _onUpSubject?.OnNext(Unit.Default);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_interactable.CurrentValue) return;

        _onEnterSubject?.OnNext(Unit.Default);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_interactable.CurrentValue) return;

        _onExitSubject?.OnNext(Unit.Default);
    }

    public void SetInteractable(bool interactable)
    {
        _interactable.Value = interactable;
    }
}
