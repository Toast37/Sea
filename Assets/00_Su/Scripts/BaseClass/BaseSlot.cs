using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSlot : ScriptableObject, ISlot
{
    protected List<ICard> _cards = new List<ICard>();
    public IReadOnlyList<ICard> Cards => _cards;
    public int? Capacity { get; set; }

    public abstract bool ShouldAddCard(ICard card);
    public abstract bool ShouldRemoveCard(ICard card);

    public virtual bool Add(ICard card)
    {
        if (!ShouldAddCard(card)) return false;
        if (Capacity.HasValue && _cards.Count >= Capacity) return false;
        _cards.Add(card);
        return true;
    }

    public virtual bool Remove(ICard card)
    {
        if (!ShouldRemoveCard(card)) return false;
        return _cards.Remove(card);
    }

    public bool Has(ICard card) => _cards.Contains(card);
}
