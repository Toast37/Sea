using UnityEngine;

public abstract class BaseCard : ScriptableObject, ICard
{
    public abstract string Name { get; }
    public abstract CardType Type { get; }
    public virtual void OnEquip() { }
    public virtual void OnUnequip() { }
}
