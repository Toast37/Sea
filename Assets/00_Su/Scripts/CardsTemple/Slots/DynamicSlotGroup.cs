using System;
using System.Collections.Generic;

public class DynamicSlotGroup : ISlotGroup
{
    private readonly List<ISlot> _slots = new();
    private readonly Func<ISlot> _slotFactory;
    private readonly int         _maxCount;

    public IList<ISlot> Slots => _slots;

    public DynamicSlotGroup(Func<ISlot> slotFactory, int maxCount = int.MaxValue)
    {
        _slotFactory = slotFactory;
        _maxCount    = maxCount;
    }

    public bool AddCard(ICard card)
    {
        if (_slots.Count >= _maxCount) return false;
        var slot = _slotFactory();
        if (!slot.Add(card)) return false;
        _slots.Add(slot);
        return true;
    }

    public bool Insert(int index, ICard card)
    {
        if (_slots.Count >= _maxCount) return false;
        var slot = _slotFactory();
        if (!slot.Add(card)) return false;
        index = Math.Clamp(index, 0, _slots.Count);
        _slots.Insert(index, slot);
        return true;
    }

    public bool Move(int from, int to)
    {
        if (from < 0 || from >= _slots.Count) return false;
        var slot = _slots[from];
        _slots.RemoveAt(from);
        to = Math.Clamp(to, 0, _slots.Count);
        _slots.Insert(to, slot);
        return true;
    }

    public bool RemoveCard(ICard card)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].Has(card))
            {
                _slots.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public IEnumerable<ICard> GetAllCards()
    {
        foreach (var slot in _slots)
            if (slot.Card != null) yield return slot.Card;
    }
}
