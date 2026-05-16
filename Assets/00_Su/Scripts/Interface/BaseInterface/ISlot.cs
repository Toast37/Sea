public interface ISlot
{
    ICard Card { get; }
    bool Add(ICard card);
    bool Remove(ICard card);
    bool Has(ICard card);
    bool ShouldAddCard(ICard card);
    bool ShouldRemoveCard(ICard card);
}
