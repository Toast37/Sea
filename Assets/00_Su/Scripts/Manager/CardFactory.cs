using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardEntry
{
    public string   Id;
    public BaseCard Card;
}

[Serializable]
public class CharacterEntry
{
    public string        Id;
    public BaseCharacter Character;
}

[Serializable]
public class SkillEntry
{
    public string    Id;
    public BaseSkill Skill;
}

public class CardFactory : BaseManager<CardFactory>
{
    [SerializeField] private List<CardEntry>      _cardEntries;
    [SerializeField] private List<CharacterEntry> _characterEntries;
    [SerializeField] private List<SkillEntry>     _skillEntries;

    private readonly Dictionary<string, ScriptableObject> _registry      = new();
    private readonly Dictionary<string, BaseSkill>        _skillRegistry = new();

    protected override void Awake()
    {
        base.Awake();
        Register(_cardEntries, _characterEntries);
        RegisterSkills(_skillEntries);
    }

    public void Register(IEnumerable<CardEntry> cards, IEnumerable<CharacterEntry> characters)
    {
        foreach (var e in cards)      _registry[e.Id] = e.Card;
        foreach (var e in characters) _registry[e.Id] = e.Character;
    }

    public void RegisterSkills(IEnumerable<SkillEntry> skills)
    {
        foreach (var e in skills) _skillRegistry[e.Id] = e.Skill;
    }

    public CardDescriptor CreateDescriptor(string cardId)
    {
        if (!_registry.TryGetValue(cardId, out var so))
        {
            Debug.LogWarning($"[CardFactory] 找不到卡牌 ID: {cardId}");
            return null;
        }
        return so is ICard card ? card.NewDescriptor(cardId) : null;
    }

    public ICard Create(CardDescriptor descriptor)
    {
        if (!_registry.TryGetValue(descriptor.CardId, out var so))
        {
            Debug.LogWarning($"[CardFactory] 找不到卡牌 ID: {descriptor.CardId}");
            return null;
        }

        var instance = UnityEngine.Object.Instantiate(so);

        if (instance is BaseCharacter ch)
        {
            ch.Init(descriptor.CardId);
            ch.Restore(descriptor);
            Debug.Log($"[CardFactory] 创建角色: {descriptor.CardId}");
            return ch;
        }
        if (instance is BaseCard bc)
        {
            bc.InitInstance(descriptor.CardId);
            bc.Restore(descriptor);
            Debug.Log($"[CardFactory] 创建卡牌: {descriptor.CardId}");
            return bc;
        }

        Debug.LogWarning($"[CardFactory] 未知类型: {descriptor.CardId}");
        return null;
    }

    public ISkill CreateSkill(string skillId)
    {
        if (!_skillRegistry.TryGetValue(skillId, out var so))
        {
            Debug.LogWarning($"[CardFactory] 找不到技能 ID: {skillId}");
            return null;
        }
        var instance = UnityEngine.Object.Instantiate(so);
        instance.InitSkill(skillId);
        Debug.Log($"[CardFactory] 创建技能: {skillId}");
        return instance;
    }
}
