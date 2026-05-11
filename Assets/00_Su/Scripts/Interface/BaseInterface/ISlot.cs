using System.Collections.Generic;

public interface ISlot
{
    IReadOnlyList<ICard> Cards { get; }
    int? Capacity { get; }
    bool Add(ICard card);
    bool Remove(ICard card);
    bool Has(ICard card);
    bool ShouldAddCard(ICard card);
    bool ShouldRemoveCard(ICard card);
}
