public class HandSlot : BaseSlot
{
    public override bool ShouldAddCard(ICard card) => true;
    public override bool ShouldRemoveCard(ICard card) => true;
}
