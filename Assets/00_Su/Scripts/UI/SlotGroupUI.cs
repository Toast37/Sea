using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSlotGroupUI : BasePanel
{
    [SerializeField] private Sprite _defaultIcon;
    [SerializeField] protected List<BaseSlotUI> _slotUIs;

    protected IReadOnlyList<BaseSlotUI> SlotUIs => _slotUIs;

    protected virtual void OnEnable()  => Init();
    protected virtual void OnDisable() => Cleanup();

    protected virtual void Init()
    {
        GameManager.Instance.OnStateChanged += Refresh;
    }

    protected virtual void Cleanup()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= Refresh;
    }

    protected virtual void Refresh() { }
}
