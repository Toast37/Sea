using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    public ICharacter        CurrentCharacter   { get; private set; }
    public List<ICharacter>  Party              { get; private set; } = new List<ICharacter>();

    public void AddToParty(ICharacter character)
    {
        (character as BaseCharacter)?.Init();
        Party.Add(character);
    }

    public void SetCurrentCharacter(ICharacter character)
    {
        CurrentCharacter = character;
        GameManager.Instance.NotifyStateChanged();
    }

    public void Equip(ICharacter character, ICard card, ISlot slot)
    {
        slot.Add(card);
        if (card is IExtra extra) ApplyExtra(character, extra, card);
        card.OnEquip(character);
        GameManager.Instance.NotifyStateChanged();
    }

    public void Unequip(ICharacter character, ICard card, ISlot slot)
    {
        slot.Remove(card);
        if (card is IExtra extra) RemoveExtra(character, extra, card);
        card.OnUnequip(character);
        GameManager.Instance.NotifyStateChanged();
    }

    public void ModifyAllStat(object source, StatType stat, int value)
    {
        foreach (var c in Party)
            c.AddMod(source, stat, value);
    }

    public void ApplyExtra(ICharacter character, IExtra extra, object source = null)
    {
        string sourceId = (source as ICard)?.CardId ?? extra.GetType().Name;
        foreach (var kv in extra.StatModifiers)
            character.AddMod(sourceId, kv.Key, kv.Value);
        foreach (var kv in extra.MetaStatModifiers)
            GameManager.Instance.MetaStats.Modify(kv.Key.ToString(), kv.Value);
    }

    public void RemoveExtra(ICharacter character, IExtra extra, object source = null)
    {
        string sourceId = (source as ICard)?.CardId ?? extra.GetType().Name;
        character.RemoveMod(sourceId);
        foreach (var kv in extra.MetaStatModifiers)
            GameManager.Instance.MetaStats.Modify(kv.Key.ToString(), -kv.Value);
    }

    public void OnCharacterDead(ICharacter character)
    {
        foreach (var skill in character.Skills)
            skill.OnExpire();
        character.Skills.Clear();
        Party.Remove(character);
        character.OnDead();
        GameManager.Instance.NotifyStateChanged();
    }
}
