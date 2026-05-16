using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModData
{
    public StatType Stat;
    public int      Value;
    public string   SourceId;
}

[Serializable]
public class CharacterDescriptor : CardDescriptor
{
    public int                  CurrentHealth;
    public List<ModData>        Mods          = new();
    [SerializeReference, ExcludeDescriptor(typeof(CharacterDescriptor))]
    public List<CardDescriptor> EquippedItems = new();
    public List<string>         SkillIds      = new();
}
