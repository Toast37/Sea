public interface ICard : IVisual
{
    string   CardId     { get; }
    string   Name       { get; }
    string         GetPanelKey();
    CardDescriptor Capture();
    void           Restore(CardDescriptor d);
    CardDescriptor NewDescriptor(string cardId);
    void OnEquip(ICharacter owner);
    void OnUnequip(ICharacter owner);
}
