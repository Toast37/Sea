public interface IEvent : IConditionOwner
{
    string Name { get; }
    EventVisibility Visibility { get; }
    int ActionCost { get; }
    StatType CheckStat { get; }
    int CheckValue { get; }
    bool RequiresRoll { get; }
    IEventResult OnSuccess { get; }
    IEventResult OnFail { get; }
}
