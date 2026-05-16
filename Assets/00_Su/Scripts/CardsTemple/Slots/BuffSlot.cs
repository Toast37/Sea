public class BuffSlot : BaseSlot
{
    public override bool ShouldAddCard(ICard card) => card is BaseBuffCard;
    public override bool ShouldRemoveCard(ICard card) => true;
}
