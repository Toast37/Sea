using System.Collections.Generic;

public class SimpleSlotGroup : ISlotGroup
{
    private List<ISlot> _slots;
    public IReadOnlyList<ISlot> Slots => _slots;

    public SimpleSlotGroup(List<ISlot> slots) => _slots = slots;

    public bool AddCard(ICard card)
    {
        foreach (var slot in _slots)
            if (slot.Add(card)) return true;
        return false;
    }

    public bool RemoveCard(ICard card)
    {
        foreach (var slot in _slots)
            if (slot.Remove(card)) return true;
        return false;
    }

    public IEnumerable<ICard> GetAllCards()
    {
        foreach (var slot in _slots)
            foreach (var card in slot.Cards)
                yield return card;
    }
}
