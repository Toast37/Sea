public interface IStats
{
    int maxHealth { get; }
    int Strength { get; }
    int Wisdom { get; }
    int Charisma { get; }
    int GetStat(StatType type);
    int GetBaseStat(StatType type);
}
