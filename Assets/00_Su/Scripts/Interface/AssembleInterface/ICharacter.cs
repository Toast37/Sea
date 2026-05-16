using System.Collections.Generic;

public interface ICharacter : ICard, IStats, IConditionOwner
{
    List<ISkill> Skills { get; }
    ISlotGroup SlotGroup { get; }
    int CurrentHealth { get; }
    void TakeDamage(int amount);
    void Heal(int amount);
    void AddMod(object source, StatType stat, int value);
    void RemoveMod(object source);
    void ApplyStat(StatType type, int value);
    void OnSpawn();
    void OnDead();
}
