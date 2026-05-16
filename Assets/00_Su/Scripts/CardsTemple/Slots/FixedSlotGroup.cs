using System.Collections.Generic;

public class FixedSlotGroup : ISlotGroup
{
    private readonly List<ISlot> _slots;

    public IList<ISlot> Slots => _slots;

    public FixedSlotGroup(List<ISlot> slots) => _slots = slots;

    public bool AddCard(ICard card)
    {
        foreach (var slot in _slots)
            if (slot.Card == null && slot.Add(card)) return true;
        return false;
    }

    public bool RemoveCard(ICard card)
    {
        foreach (var slot in _slots)
            if (slot.Has(card)) return slot.Remove(card);
        return false;
    }

    public IEnumerable<ICard> GetAllCards()
    {
        foreach (var slot in _slots)
            if (slot.Card != null) yield return slot.Card;
    }
}
