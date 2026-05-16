using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class HandSlotUI : BaseSlotUI,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerDownHandler, IPointerUpHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _moveDuration = 0.2f;

    private HandGroupUI _group;
    private CanvasGroup _canvasGroup;
    private Vector3     _dragOffset;
    private Tweener     _tween;
    private Tweener     _scaleTween;
    [SerializeField] private float _longPressThreshold = 0.2f;
    private float _pointerDownTime;
    private bool  _isDragMode;
    private bool  _dragging;

    protected override void Awake()
    {
        base.Awake();
        _group       = GetComponentInParent<HandGroupUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _rect        = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownTime = Time.time;
        _isDragMode      = false;
        _dragging        = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_dragging && Time.time - _pointerDownTime < _longPressThreshold)
            OnClick();
        _isDragMode = false;
    }

    protected virtual void OnClick()
    {
        if (_slot?.Card != null)
            UIManager.Instance.Show(_slot.Card.GetPanelKey(), _slot.Card);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Time.time - _pointerDownTime < _longPressThreshold) return;
        _isDragMode = true;
        _dragging   = true;
        _tween?.Kill();
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(1f, _moveDuration).SetEase(Ease.OutQuad);
        _group.OnCardHoverExit();
        _tween?.Kill();
        _dragOffset = transform.position - (Vector3)eventData.position;
        _group.BeginDrag(this, eventData);
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = false;
    }

    [SerializeField] private float _dragLerpSpeed = 15f;
    private RectTransform _rect;
    private Vector3       _dragTargetPos;
    private Coroutine     _hoverCoroutine;

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragMode) return;
        _dragTargetPos = (Vector3)eventData.position + _dragOffset;
        _group.OnCardDrag(eventData);
    }

    private void Update()
    {
        if (_isDragMode)
            transform.position = Vector3.Lerp(transform.position, _dragTargetPos, Time.deltaTime * _dragLerpSpeed);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (_isDragMode || _group.IsDragging) return;
        _hoverCoroutine = StartCoroutine(HoverDelay());
    }

    private IEnumerator HoverDelay()
    {
        yield return new WaitForSeconds(_group.HoverDelay);
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(_group.HoverScale, _moveDuration).SetEase(Ease.OutQuad)
            .OnComplete(OnHoverComplete);
        _group.OnCardHover(this, _rect.rect.width);
    }

    protected virtual void OnHoverComplete() { }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (_hoverCoroutine != null) { StopCoroutine(_hoverCoroutine); _hoverCoroutine = null; }
        if (_group.IsDragging) return;
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(1f, _moveDuration).SetEase(Ease.OutQuad);
        _group.OnCardHoverExit();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_dragging) return;
        _isDragMode = false;
        _dragging   = false;
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = true;
        _group.EndDrag(this);
    }

    public void MoveTo(Vector3 targetLocalPos, float delay = 0f)
    {
        _tween?.Kill();
        _tween = transform.DOLocalMove(targetLocalPos, _moveDuration)
            .SetEase(Ease.OutQuad)
            .SetDelay(delay)
            .OnStart(   () => { if (_canvasGroup != null) _canvasGroup.blocksRaycasts = false; })
            .OnComplete(() => { if (_canvasGroup != null) _canvasGroup.blocksRaycasts = true;  });
    }
}
