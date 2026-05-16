using System;
using UnityEngine;

public abstract class BaseCard : ScriptableObject, ICard
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    [SerializeField] private string _panelKey = "CardPanel";

    [NonSerialized] private string _cardId;

    public           string   CardId      => _cardId;
    public abstract  string   Name        { get; }
    public virtual   Sprite   Icon        => _icon;
    public virtual   string   Description => _description;

    public virtual string GetPanelKey() => _panelKey;

    public void InitInstance(string cardId) => _cardId = cardId;

    public virtual CardDescriptor Capture() => new CardDescriptor { CardId = _cardId };

    public virtual CardDescriptor NewDescriptor(string cardId)
        => new CardDescriptor { CardId = cardId };

    public virtual void Restore(CardDescriptor d) { }

    protected ICharacter _owner;
    public    ICharacter Owner => _owner;

    public virtual void OnEquip(ICharacter owner)   { _owner = owner; }
    public virtual void OnUnequip(ICharacter owner) { _owner = null; }
}
