using System;
using UnityEngine;

public class GameManager : BaseManager<GameManager>
{
    public DayState DayState { get; private set; } = new DayState();
    public TagManager TagManager { get; private set; } = new TagManager();
    public MetaStats MetaStats { get; private set; } = new MetaStats();

    public event Action OnStateChanged;
    public void NotifyStateChanged() => OnStateChanged?.Invoke();

    public GameSnapshot TakeSnapshot()
    {
        return new GameSnapshot
        {
            CurrentCharacter = CharacterManager.Instance.CurrentCharacter,
            Party = CharacterManager.Instance.Party,
            DayState = DayState,
            TagManager = TagManager,
            MetaStats = MetaStats
        };
    }
}
