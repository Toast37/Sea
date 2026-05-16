using System.Collections;

public class DayLoop
{
    public IEnumerator Run(ICharacter character)
    {
        yield return AddDailyBuff(character);

        foreach (var skill in character.Skills)
            skill.OnTick();

        foreach (var skill in GameManager.Instance.MetaStats.Skills)
            skill.OnTick();

        yield return ProcessExpired(character);

        GameManager.Instance.DayState.NextDay();

        EventManager.Instance.RefreshCurrentPool();
        ConditionChecker.Instance.Check();

        yield return WaitForPlayerActions();

        OnDayEnd(character);
    }

    private IEnumerator AddDailyBuff(ICharacter character) { yield break; }
    private IEnumerator ProcessExpired(ICharacter character) { yield break; }
    private IEnumerator WaitForPlayerActions() { yield break; }
    private void OnDayEnd(ICharacter character) { }
}
