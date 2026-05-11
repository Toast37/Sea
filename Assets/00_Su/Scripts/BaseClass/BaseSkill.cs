using UnityEngine;

public abstract class BaseSkill : ScriptableObject, ISkill
{
    public virtual void OnGain() { }
    public virtual void OnTick() { }
    public virtual void OnExpire() { }
    public virtual void OnRoll() { }
}
