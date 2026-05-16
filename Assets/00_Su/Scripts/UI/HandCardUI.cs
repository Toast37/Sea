using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardUI : HandSlotUI,
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image           _icon;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private float           _fadeDuration = 0.15f;

    private Tweener _fadeTween;

    public override void Show()
    {
        base.Show();
        if (_icon        != null) _icon.sprite = _slot?.Card?.Icon;
        if (_description != null)
        {
            _description.text  = string.Empty;
            _description.alpha = 0f;
        }
    }

    protected override void OnHoverComplete()
    {
        if (_description == null || _slot?.Card == null) return;
        _description.text = _slot.Card.Description;
        _fadeTween?.Kill();
        _fadeTween = _description.DOFade(1f, _fadeDuration);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (_description == null) return;
        _fadeTween?.Kill();
        _fadeTween = _description.DOFade(0f, _fadeDuration)
            .OnComplete(() => _description.text = string.Empty);
    }
}
