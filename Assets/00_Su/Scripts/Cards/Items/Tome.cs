using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tome")]
public class Tome : BaseItemCard
{
    public override string Name => "Tome";
    public override Dictionary<StatType, int> StatModifiers { get; } = new Dictionary<StatType, int>
    {
        { StatType.Wisdom, 2 }
    };
}
