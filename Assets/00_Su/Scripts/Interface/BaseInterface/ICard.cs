public interface ICard
{
    string Name { get; }
    CardType Type { get; }
    void OnEquip();
    void OnUnequip();
}
