using System.Collections.Generic;

public interface IEventResult
{
    List<GameCommand> Commands { get; }
}
