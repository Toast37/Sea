using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuffCard : BaseCard, IExtra
{
    public override CardType Type => CardType.Buff;
    public virtual Dictionary<StatType, int> StatModifiers { get; } = new Dictionary<StatType, int>();
    public virtual Dictionary<MetaStatType, int> MetaStatModifiers { get; } = new Dictionary<MetaStatType, int>();

    [SerializeField] private List<BaseSkill> _skills;

    public override void OnEquip()
    {
        var target = CharacterManager.Instance.EquippingCharacter;
        if (target == null) return;
        foreach (var skill in _skills)
            target.Skills.Add(skill);
    }

    public override void OnUnequip()
    {
        var target = CharacterManager.Instance.EquippingCharacter;
        if (target == null) return;
        foreach (var skill in _skills)
            target.Skills.Remove(skill);
    }
}
