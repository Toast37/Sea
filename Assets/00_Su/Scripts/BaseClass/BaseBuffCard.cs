using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuffCard : BaseCard, IExtra
{
    [Serializable]
    private struct StatEntry
    {
        public StatType Stat;
        public int      Value;
    }

    [Serializable]
    private struct MetaStatEntry
    {
        public MetaStatType Stat;
        public int          Value;
    }

    [SerializeField] private List<StatEntry>     _statModifiers     = new List<StatEntry>();
    [SerializeField] private List<MetaStatEntry> _metaStatModifiers = new List<MetaStatEntry>();


    private Dictionary<StatType, int>     _statModifierCache;
    private Dictionary<MetaStatType, int> _metaStatModifierCache;

    public Dictionary<StatType, int> StatModifiers
    {
        get
        {
            if (_statModifierCache != null) return _statModifierCache;
            _statModifierCache = new Dictionary<StatType, int>();
            foreach (var e in _statModifiers) _statModifierCache[e.Stat] = e.Value;
            return _statModifierCache;
        }
    }

    public Dictionary<MetaStatType, int> MetaStatModifiers
    {
        get
        {
            if (_metaStatModifierCache != null) return _metaStatModifierCache;
            _metaStatModifierCache = new Dictionary<MetaStatType, int>();
            foreach (var e in _metaStatModifiers) _metaStatModifierCache[e.Stat] = e.Value;
            return _metaStatModifierCache;
        }
    }

    [SerializeField] private List<BaseSkill> _skills;

    public override void OnEquip(ICharacter owner)
    {
        base.OnEquip(owner);
        if (owner == null) return;
        foreach (var skill in _skills)
            if (!owner.Skills.Contains(skill))
            {
                owner.Skills.Add(skill);
                skill.OnGain();
            }
    }

    public override void OnUnequip(ICharacter owner)
    {
        if (owner == null) { base.OnUnequip(owner); return; }
        foreach (var skill in _skills)
            owner.Skills.Remove(skill);
        base.OnUnequip(owner);
    }
}
