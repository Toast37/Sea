public interface IStats
{
    int Strength { get; }
    int Wisdom { get; }
    int Charisma { get; }
    int GetStat(StatType type);
}
