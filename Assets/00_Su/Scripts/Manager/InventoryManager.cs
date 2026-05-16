using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : BaseManager<InventoryManager>
{
    [SerializeReference] private List<CardDescriptor> _initialDescriptors;

    private readonly List<ISlot>          _slots       = new();
    private readonly List<CardDescriptor> _descriptors = new();

    public IReadOnlyList<ISlot> Slots => _slots;

    protected override void Awake()
    {
        base.Awake();
        Load(_initialDescriptors);
    }

    private void Load(IEnumerable<CardDescriptor> descriptors)
    {
        _slots.Clear();
        _descriptors.Clear();
        foreach (var d in descriptors)
            AddInternal(d, CardFactory.Instance.Create(d));
    }

    public ICard Add(CardDescriptor descriptor)
    {
        var card = CardFactory.Instance.Create(descriptor);
        AddInternal(descriptor, card);
        GameManager.Instance.NotifyStateChanged();
        return card;
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= _slots.Count) return;
        _slots.RemoveAt(index);
        _descriptors.RemoveAt(index);
        GameManager.Instance.NotifyStateChanged();
    }

    public void Reorder(int from, int to)
    {
        if (from < 0 || from >= _slots.Count) return;
        var slot = _slots[from];
        var desc = _descriptors[from];
        _slots.RemoveAt(from);
        _descriptors.RemoveAt(from);
        to = Mathf.Clamp(to, 0, _slots.Count);
        _slots.Insert(to, slot);
        _descriptors.Insert(to, desc);
    }

    public List<CardDescriptor> Save()
    {
        var result = new List<CardDescriptor>();
        foreach (var slot in _slots)
            if (slot.Card != null) result.Add(slot.Card.Capture());
        return result;
    }

    private void AddInternal(CardDescriptor descriptor, ICard card)
    {
        var slot = new InventorySlot();
        slot.Add(card);
        _slots.Add(slot);
        _descriptors.Add(descriptor);
    }
}

public class InventorySlot : BaseSlot
{
    public override bool ShouldAddCard(ICard card)    => true;
    public override bool ShouldRemoveCard(ICard card) => true;
}
