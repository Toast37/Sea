public class ItemSlot : BaseSlot
{
    public override bool ShouldAddCard(ICard card) => card.Type == CardType.Item;
    public override bool ShouldRemoveCard(ICard card) => true;
}
