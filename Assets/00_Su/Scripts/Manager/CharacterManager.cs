using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    public ICharacter CurrentCharacter { get; private set; }
    public ICharacter EquippingCharacter { get; private set; }
    public List<ICharacter> Party { get; private set; } = new List<ICharacter>();

    public void SetCurrentCharacter(ICharacter character)
    {
        CurrentCharacter = character;
        GameManager.Instance.NotifyStateChanged();
    }

    public void Equip(ICharacter character, ICard card, ISlot slot)
    {
        EquippingCharacter = character;
        slot.Add(card);
        if (card is IExtra extra) ApplyExtra(character, extra);
        card.OnEquip();
        EquippingCharacter = null;
        GameManager.Instance.NotifyStateChanged();
    }

    public void Unequip(ICharacter character, ICard card, ISlot slot)
    {
        EquippingCharacter = character;
        slot.Remove(card);
        if (card is IExtra extra) RemoveExtra(character, extra);
        card.OnUnequip();
        EquippingCharacter = null;
        GameManager.Instance.NotifyStateChanged();
    }

    public void ModifyStat(ICharacter character, StatType stat, int value)
    {
        character.ApplyStat(stat, value);
    }

    public void ModifyAllStat(StatType stat, int value)
    {
        foreach (var c in Party)
            ModifyStat(c, stat, value);
    }

    public void ApplyExtra(ICharacter character, IExtra extra)
    {
        foreach (var kv in extra.StatModifiers)
            ModifyStat(character, kv.Key, kv.Value);
        foreach (var kv in extra.MetaStatModifiers)
            GameManager.Instance.MetaStats.Modify(kv.Key.ToString(), kv.Value);
    }

    public void RemoveExtra(ICharacter character, IExtra extra)
    {
        foreach (var kv in extra.StatModifiers)
            ModifyStat(character, kv.Key, -kv.Value);
        foreach (var kv in extra.MetaStatModifiers)
            GameManager.Instance.MetaStats.Modify(kv.Key.ToString(), -kv.Value);
    }

    public void OnCharacterDead(ICharacter character)
    {
        foreach (var skill in character.Skills)
            skill.OnExpire();
        character.Skills.Clear();
        Party.Remove(character);
        GameManager.Instance.NotifyStateChanged();
    }
}
