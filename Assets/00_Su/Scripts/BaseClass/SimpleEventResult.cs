using System.Collections.Generic;

public class SimpleEventResult : IEventResult
{
    public List<GameCommand> Commands { get; }
    public SimpleEventResult(List<GameCommand> commands) => Commands = commands;
}
