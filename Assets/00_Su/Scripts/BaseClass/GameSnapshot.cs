using System.Collections.Generic;

public struct GameSnapshot
{
    public ICharacter CurrentCharacter;
    public List<ICharacter> Party;
    public DayState DayState;
    public TagManager TagManager;
    public MetaStats MetaStats;
}
