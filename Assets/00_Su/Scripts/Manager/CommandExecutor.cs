using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandExecutor : BaseManager<CommandExecutor>
{
    private Dictionary<CommandType, Action<GameCommand>> _handlers;

    private Dictionary<CommandType, Action<GameCommand>> Handlers
    {
        get
        {
            if (_handlers == null)
            {
                _handlers = new Dictionary<CommandType, Action<GameCommand>>
                {
                    { CommandType.AddTag,       cmd => GameManager.Instance.TagManager.Add(cmd.Param) },
                    { CommandType.RemoveTag,    cmd => GameManager.Instance.TagManager.Remove(cmd.Param) },
                    { CommandType.AddCard,      cmd => InventoryManager.Instance.Add(cmd.Descriptor) },
                    { CommandType.RemoveCard,   cmd => InventoryManager.Instance.Remove(cmd.Value) },
                    { CommandType.EquipCard,    cmd => { } },
                    { CommandType.UnequipCard,  cmd => { } },
                    { CommandType.StatDelta,    cmd => CharacterManager.Instance.CurrentCharacter?
                        .AddMod(cmd, cmd.StatType, cmd.Value) },
                    { CommandType.AllStatDelta, cmd => CharacterManager.Instance.ModifyAllStat(
                        cmd, cmd.StatType, cmd.Value) },
                    { CommandType.MetaStatDelta,cmd => GameManager.Instance.MetaStats.Modify(
                        cmd.Param, cmd.Value) },
                    { CommandType.PlayDialog,   cmd => { } },
                };
            }
            return _handlers;
        }
    }

    public void Execute(GameCommand command)
    {
        if (Handlers.TryGetValue(command.Type, out var handler))
            handler(command);
    }

    public void Register(CommandType type, Action<GameCommand> handler)
    {
        Handlers[type] = handler;
    }
}
