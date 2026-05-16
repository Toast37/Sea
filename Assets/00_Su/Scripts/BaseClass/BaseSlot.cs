public abstract class BaseSlot : ISlot
{
    private ICard _card;
    public ICard Card => _card;

    public abstract bool ShouldAddCard(ICard card);
    public abstract bool ShouldRemoveCard(ICard card);

    public virtual bool Add(ICard card)
    {
        if (_card != null) return false;
        if (!ShouldAddCard(card)) return false;
        _card = card;
        return true;
    }

    public virtual bool Remove(ICard card)
    {
        if (_card != card) return false;
        if (!ShouldRemoveCard(card)) return false;
        _card = null;
        return true;
    }

    public bool Has(ICard card) => _card == card;
}
