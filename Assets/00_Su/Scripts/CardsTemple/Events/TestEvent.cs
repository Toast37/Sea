using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/TestEvent")]
public class TestEvent : BaseEvent
{
    public override string Name => "FindGold";
    public override EventVisibility Visibility => EventVisibility.Hinted;
    public override int ActionCost => 1;
    public override StatType CheckStat => StatType.Wisdom;
    public override int CheckValue => 10;
    public override bool RequiresRoll => true;

    public override IEventResult OnSuccess => new SimpleEventResult(new List<GameCommand>
    {
        new GameCommand { Type = CommandType.MetaStatDelta, Param = "Money", Value = 10 }
    });

    public override IEventResult OnFail => new SimpleEventResult(new List<GameCommand>());

    public override void OnConditionMet() { }
}
