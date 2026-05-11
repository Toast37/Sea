using System;

public interface IReactiveCondition : ICondition
{
    Action OnMet { get; }
    Action OnNotMet { get; }
    Action OnChanged { get; }
}
