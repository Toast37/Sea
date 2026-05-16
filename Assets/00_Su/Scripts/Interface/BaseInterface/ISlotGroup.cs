using System.Collections.Generic;

public interface ISlotGroup
{
    IList<ISlot> Slots { get; }
    bool AddCard(ICard card);
    bool RemoveCard(ICard card);
    IEnumerable<ICard> GetAllCards();
}
