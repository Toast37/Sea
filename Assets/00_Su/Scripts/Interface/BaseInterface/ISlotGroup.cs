using System.Collections.Generic;

public interface ISlotGroup
{
    IReadOnlyList<ISlot> Slots { get; }
    bool AddCard(ICard card);
    bool RemoveCard(ICard card);
    IEnumerable<ICard> GetAllCards();
}
