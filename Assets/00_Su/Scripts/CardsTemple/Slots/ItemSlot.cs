public class ItemSlot : BaseSlot
{
    public override bool ShouldAddCard(ICard card) => card is BaseItemCard;
    public override bool ShouldRemoveCard(ICard card) => true;
}
