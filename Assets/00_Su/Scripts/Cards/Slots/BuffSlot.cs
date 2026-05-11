public class BuffSlot : BaseSlot
{
    public override bool ShouldAddCard(ICard card) => card.Type == CardType.Buff;
    public override bool ShouldRemoveCard(ICard card) => true;
}
