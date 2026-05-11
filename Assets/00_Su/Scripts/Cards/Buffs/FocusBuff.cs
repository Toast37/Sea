using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/FocusBuff")]
public class FocusBuff : BaseBuffCard
{
    public override string Name => "Focus";
    public override Dictionary<StatType, int> StatModifiers { get; } = new Dictionary<StatType, int>
    {
        { StatType.Wisdom, 4 }
    };
}
