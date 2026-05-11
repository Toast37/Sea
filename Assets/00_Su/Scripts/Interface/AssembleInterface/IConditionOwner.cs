using System.Collections.Generic;

public interface IConditionOwner
{
    List<ICondition> RequireAll { get; }
    List<IReactiveCondition> RequireAny { get; }
    void OnConditionMet();
}
