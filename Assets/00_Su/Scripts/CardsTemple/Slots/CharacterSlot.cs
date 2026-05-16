public class CharacterSlot : BaseSlot
{
    public override bool ShouldAddCard(ICard card) => card is ICharacter;
    public override bool ShouldRemoveCard(ICard card) => true;
}
