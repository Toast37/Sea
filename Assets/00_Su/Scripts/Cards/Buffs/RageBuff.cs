using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/RageBuff")]
public class RageBuff : BaseBuffCard
{
    public override string Name => "Rage";
    public override Dictionary<StatType, int> StatModifiers { get; } = new Dictionary<StatType, int>
    {
        { StatType.Strength, 4 }
    };
}
