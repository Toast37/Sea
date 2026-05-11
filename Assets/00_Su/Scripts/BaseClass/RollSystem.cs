using UnityEngine;

public static class RollSystem
{
    public static RollResult Roll(ICharacter character, IEvent eventData)
    {
        var context = new RollContext();

        foreach (var skill in character.Skills)
            skill.OnRoll();

        int statValue = character.GetStat(eventData.CheckStat);
        int baseRoll  = Random.Range(1, 21);
        var dayState = GameManager.Instance.DayState;
        if (dayState.ActionPoints < dayState.MaxActionPoints / 2)
            baseRoll /= 2;
        int additive  = baseRoll + statValue + context.TotalBonus - context.TotalPenalty;
        int total     = additive * context.TotalMultiplier;



        return new RollResult
        {
            Base      = baseRoll,
            StatValue = statValue,
            Total     = total,
            Outcome   = GetOutcome(total, eventData.CheckValue)
        };
    }

    private static RollOutcome GetOutcome(int total, int checkValue)
    {
        return total >= checkValue ? RollOutcome.Success : RollOutcome.Fail;
    }
}
