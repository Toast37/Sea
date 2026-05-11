public enum CardType
{
    Buff,
    Item,
    Character
}

public enum StatType
{
    Strength,
    Wisdom,
    Charisma
}

public enum MetaStatType
{
    Money,
    Luck
}

public enum RollOutcome
{
    Success,
    Fail
}

public enum CommandType
{
    AddTag,
    RemoveTag,
    AddCard,
    RemoveCard,
    EquipCard,
    UnequipCard,
    StatDelta,
    AllStatDelta,
    MetaStatDelta,
    PlayDialog
}

public enum EventVisibility
{
    Hidden,
    Teased,
    Hinted
}
