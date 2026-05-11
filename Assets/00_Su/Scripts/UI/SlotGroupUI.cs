using System.Collections.Generic;
using UnityEngine;

public class SlotGroupUI : MonoBehaviour
{
    [SerializeField] private Sprite _defaultIcon;
    [SerializeField] private List<BaseSlotUI> _slotUIs;
    private ISlotGroup _slotGroup;

    public void Init(ISlotGroup slotGroup)
    {
        _slotGroup = slotGroup;
        GameManager.Instance.OnStateChanged += Refresh;
        for (int i = 0; i < _slotUIs.Count; i++)
            _slotUIs[i].Init(_slotGroup.Slots[i], _defaultIcon);
    }

    private void Refresh()
    {
        for (int i = 0; i < _slotGroup.Slots.Count; i++)
            _slotUIs[i].Refresh(_slotGroup.Slots[i]);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= Refresh;
    }
}
