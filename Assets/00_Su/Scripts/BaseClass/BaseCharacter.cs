using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : ScriptableObject, ICharacter
{
    private struct StatMod
    {
        public object   Source;
        public StatType Stat;
        public int      Value;
    }

    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    [SerializeField] private string _panelKey;
    [SerializeField] private int    _maxHealth;
    [SerializeField] private int    _strength;
    [SerializeField] private int    _wisdom;
    [SerializeField] private int    _charisma;
    [SerializeField] private int    _equipmentSlotCount = 3;

    public virtual string   Name        => _name;
    public virtual Sprite   Icon        => _icon;
    public virtual string   Description => _description;
    [NonSerialized] private string _cardId;

    public         string   CardId      => _cardId;
    public virtual string GetPanelKey() => _panelKey;
    public virtual int      maxHealth => _maxHealth;
    public virtual int      Strength  => _strength;
    public virtual int      Wisdom    => _wisdom;
    public virtual int      Charisma  => _charisma;

    // ── 运行时状态 ──────────────────────────────────────────
    public int CurrentHealth { get; private set; }

    private ISlotGroup       _slotGroup;
    private List<StatMod>    _mods = new List<StatMod>();

    public ISlotGroup SlotGroup => _slotGroup;

    // ── 初始化（Instantiate 后调用）─────────────────────────
    public void Init(string cardId = null)
    {
        if (cardId != null) _cardId = cardId;
        CurrentHealth = maxHealth;
        _slotGroup    = CreateSlotGroup();
        _mods         = new List<StatMod>();
        Skills.Clear();
        if (_innateSkills != null)
            foreach (var skill in _innateSkills)
                if (!Skills.Contains(skill)) Skills.Add(skill);
    }

    protected virtual ISlotGroup CreateSlotGroup() =>
        new DynamicSlotGroup(() => new ItemSlot(), _equipmentSlotCount);

    // ── 持久化 ───────────────────────────────────────────────
    public CardDescriptor NewDescriptor(string cardId)
        => new CharacterDescriptor
        {
            CardId        = cardId,
            CurrentHealth = _maxHealth,
            Mods          = new List<ModData>(),
            EquippedItems = new List<CardDescriptor>(),
            SkillIds      = new List<string>(),
        };

    public CardDescriptor Capture()
    {
        var mods = new List<ModData>();
        foreach (var m in _mods)
            mods.Add(new ModData { Stat = m.Stat, Value = m.Value, SourceId = m.Source as string });

        var equipped = new List<CardDescriptor>();
        foreach (var slot in _slotGroup.Slots)
            equipped.Add(slot.Card?.Capture());

        var skillIds = new List<string>();
        foreach (var skill in Skills)
            if (skill is BaseSkill bs && bs.SkillId != null) skillIds.Add(bs.SkillId);

        return new CharacterDescriptor
        {
            CardId        = _cardId,
            CurrentHealth = CurrentHealth,
            Mods          = mods,
            EquippedItems = equipped,
            SkillIds      = skillIds,
        };
    }

    public void Restore(CardDescriptor d)
    {
        if (d is not CharacterDescriptor cd) return;
        CurrentHealth = cd.CurrentHealth;
        _mods.Clear();
        foreach (var m in cd.Mods)
            _mods.Add(new StatMod { Stat = m.Stat, Value = m.Value, Source = m.SourceId });

        for (int i = 0; i < cd.EquippedItems.Count && i < _slotGroup.Slots.Count; i++)
        {
            var itemDesc = cd.EquippedItems[i];
            if (itemDesc == null) continue;
            var card = CardFactory.Instance.Create(itemDesc);
            if (card != null) _slotGroup.Slots[i].Add(card);
        }

        foreach (var skillId in cd.SkillIds)
        {
            var skill = CardFactory.Instance.CreateSkill(skillId);
            if (skill != null && !Skills.Contains(skill))
            {
                Skills.Add(skill);
                skill.OnGain();
            }
        }
    }

    // ── 血量 ────────────────────────────────────────────────
    public void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        GameManager.Instance.NotifyStateChanged();
        if (CurrentHealth == 0) OnDead();
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        GameManager.Instance.NotifyStateChanged();
    }

    // ── Mod ─────────────────────────────────────────────────
    public void AddMod(object source, StatType stat, int value) =>
        _mods.Add(new StatMod { Source = source, Stat = stat, Value = value });

    public void RemoveMod(object source) =>
        _mods.RemoveAll(m => m.Source == source);

    public int GetStat(StatType type) => GetBaseStat(type) + GetModTotal(type);

    private int GetModTotal(StatType type)
    {
        int total = 0;
        foreach (var m in _mods)
            if (m.Stat == type) total += m.Value;
        return total;
    }

    public virtual int GetBaseStat(StatType type)
    {
        switch (type)
        {
            case StatType.Strength: return Strength;
            case StatType.Wisdom:   return Wisdom;
            case StatType.Charisma: return Charisma;
            default:                return 0;
        }
    }

    public void ApplyStat(StatType type, int value) =>
        AddMod(this, type, value);

    // ── 技能 ────────────────────────────────────────────────
    [SerializeField] private List<BaseSkill> _innateSkills;
    public List<ISkill> Skills { get; } = new List<ISkill>();

    // ── 条件 ────────────────────────────────────────────────
    public List<ICondition>         RequireAll { get; } = new List<ICondition>();
    public List<IReactiveCondition> RequireAny { get; } = new List<IReactiveCondition>();

    // ── 回调 ────────────────────────────────────────────────
    public virtual void OnEquip(ICharacter owner)        { }
    public virtual void OnUnequip(ICharacter owner)      { }
    public virtual void OnSpawn()        { }
    public virtual void OnDead()         { }
    public virtual void OnConditionMet() { }
}
