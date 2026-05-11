using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Sword")]
public class Sword : BaseItemCard
{
    public override string Name => "Sword";
    public override Dictionary<StatType, int> StatModifiers { get; } = new Dictionary<StatType, int>
    {
        { StatType.Strength, 2 }
    };
}
