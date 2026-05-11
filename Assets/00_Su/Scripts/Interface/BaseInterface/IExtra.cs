using System.Collections.Generic;

public interface IExtra
{
    Dictionary<StatType, int> StatModifiers { get; }
    Dictionary<MetaStatType, int> MetaStatModifiers { get; }
}
