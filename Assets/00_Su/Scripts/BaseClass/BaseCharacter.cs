using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : ScriptableObject, ICharacter
{
    public abstract string Name { get; }
    public CardType Type => CardType.Character;

    public abstract int Strength { get; }
    public abstract int Wisdom { get; }
    public abstract int Charisma { get; }

    private int _strengthMod;
    private int _wisdomMod;
    private int _charismaMod;

    public int GetStat(StatType type)
    {
        switch (type)
        {
            case StatType.Strength: return Strength + _strengthMod;
            case StatType.Wisdom:   return Wisdom   + _wisdomMod;
            case StatType.Charisma: return Charisma + _charismaMod;
            default:                return 0;
        }
    }

    public void ApplyStat(StatType type, int value)
    {
        switch (type)
        {
            case StatType.Strength: _strengthMod += value; break;
            case StatType.Wisdom:   _wisdomMod   += value; break;
            case StatType.Charisma: _charismaMod += value; break;
        }
    }

    [SerializeField] private List<BaseSkill> _innateSkills;
    public List<ISkill> Skills { get; } = new List<ISkill>();

    private ISlotGroup _slotGroup;
    public ISlotGroup SlotGroup => _slotGroup;

    protected virtual ISlotGroup CreateSlotGroup() =>
        new SimpleSlotGroup(new List<ISlot> { new ItemSlot() });

    private void OnEnable()
    {
        Skills.Clear();
        if (_innateSkills != null)
            foreach (var skill in _innateSkills)
                Skills.Add(skill);
        _slotGroup = CreateSlotGroup();
    }

    public List<ICondition> RequireAll { get; } = new List<ICondition>();
    public List<IReactiveCondition> RequireAny { get; } = new List<IReactiveCondition>();

    public virtual void OnEquip() { }
    public virtual void OnUnequip() { }
    public abstract void OnSpawn();
    public abstract void OnDead();
    public virtual void OnConditionMet() { }
}
