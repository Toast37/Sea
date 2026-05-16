using UnityEngine;

public abstract class BaseSlotUI : MonoBehaviour
{
    [SerializeField] protected Sprite _defaultIcon;
    public ISlot _slot;
    protected bool _isShowing;

    protected virtual void Awake() { }

    public void Init(ISlot slot, Sprite fallbackIcon = null)
    {
        _slot = slot;
        if (_defaultIcon == null) _defaultIcon = fallbackIcon;
    }

    public void Refresh(ISlot slot)
    {
        _slot = slot;
        if (_isShowing) Show();
    }

    public virtual void Show()
    {
        _isShowing = true;
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        _isShowing = false;
        gameObject.SetActive(false);
    }

    protected IVisual GetVisual() => _slot?.Card as IVisual;
}
