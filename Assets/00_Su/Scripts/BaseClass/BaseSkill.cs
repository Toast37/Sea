using System;
using UnityEngine;

public abstract class BaseSkill : ScriptableObject, ISkill
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;

    [NonSerialized] private string _skillId;

    public string SkillId => _skillId;
    public void   InitSkill(string skillId) => _skillId = skillId;

    public virtual Sprite Icon        => _icon;
    public virtual string Description => _description;

    public virtual void OnGain()   { }
    public virtual void OnTick()   { }
    public virtual void OnExpire() { }
    public virtual void OnRoll()   { }
}
