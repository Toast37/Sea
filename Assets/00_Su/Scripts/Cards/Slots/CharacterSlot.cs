public class CharacterSlot : BaseSlot
{
    public override bool ShouldAddCard(ICard card) => card.Type == CardType.Character;
    public override bool ShouldRemoveCard(ICard card) => true;
}
