using UnityEngine;

public abstract class BaseSlotUI : MonoBehaviour
{
    [SerializeField] protected Sprite _defaultIcon;
    protected ISlot _slot;
    protected bool _isShowing;

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
    }

    public virtual void Hide()
    {
        _isShowing = false;
    }
}
