using System.Collections.Generic;
using System.Linq;

public struct RollModifier
{
    public int Bonus;
    public int Penalty;
    public int Multiplier;
}

public struct RollResult
{
    public int Base;
    public int StatValue;
    public int Total;
    public RollOutcome Outcome;
}

public class RollContext
{
    private List<int> _bonuses     = new List<int>();
    private List<int> _penalties   = new List<int>();
    private List<int> _multipliers = new List<int>();

    public void AddBonus(int value)      => _bonuses.Add(value);
    public void AddPenalty(int value)    => _penalties.Add(value);
    public void AddMultiplier(int value) => _multipliers.Add(value);

    public int TotalBonus      => _bonuses.Sum();
    public int TotalPenalty    => _penalties.Sum();
    public int TotalMultiplier => _multipliers.Aggregate(1, (a, b) => a * b);
}
