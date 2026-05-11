using System.Collections.Generic;

public interface ICharacter : ICard, IStats, IConditionOwner
{
    List<ISkill> Skills { get; }
    ISlotGroup SlotGroup { get; }
    void ApplyStat(StatType type, int value);
    void OnSpawn();
    void OnDead();
}
