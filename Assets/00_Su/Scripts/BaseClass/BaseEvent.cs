using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent : ScriptableObject, IEvent
{
    public abstract string Name { get; }
    public abstract EventVisibility Visibility { get; }
    public abstract int ActionCost { get; }
    public abstract StatType CheckStat { get; }
    public abstract int CheckValue { get; }
    public abstract bool RequiresRoll { get; }
    public virtual int[] AvailableOnDays => null;
    public abstract IEventResult OnSuccess { get; }
    public abstract IEventResult OnFail { get; }
    public List<ICondition> RequireAll { get; } = new List<ICondition>();
    public List<IReactiveCondition> RequireAny { get; } = new List<IReactiveCondition>();
    public abstract void OnConditionMet();
}
