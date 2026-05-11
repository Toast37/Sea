using System.Collections.Generic;
using UnityEngine;

public class EventManager : BaseManager<EventManager>
{
    [SerializeField] private List<BaseEvent> _permanentPool;
    [SerializeField] private List<BaseEvent> _randomPool;
    [SerializeField] private List<BaseEvent> _scheduledPool;
    [SerializeField] private List<BaseEvent> _currentPool;

    public List<IEvent> PermanentPool  { get; private set; } = new List<IEvent>();
    public List<IEvent> RandomPool     { get; private set; } = new List<IEvent>();
    public List<IEvent> ScheduledPool  { get; private set; } = new List<IEvent>();
    public List<IEvent> CurrentPool    { get; private set; } = new List<IEvent>();

    private Dictionary<string, List<IEvent>> _tagEventIndex = new Dictionary<string, List<IEvent>>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var e in _permanentPool)  PermanentPool.Add(e);
        foreach (var e in _randomPool)     RandomPool.Add(e);
        foreach (var e in _scheduledPool)  ScheduledPool.Add(e);
        foreach (var e in _currentPool)    CurrentPool.Add(e);
    }

    public void AddToCurrentPool(IEvent e)
    {
        if (!CurrentPool.Contains(e))
            CurrentPool.Add(e);
    }

    public void RefreshCurrentPool()
    {
        CurrentPool.Clear();
        foreach (var e in PermanentPool)
            CurrentPool.Add(e);
    }

    public void OnTagAdded(string tag)
    {
        if (_tagEventIndex.TryGetValue(tag, out var events))
            foreach (var e in events)
                ConditionChecker.Instance.Register(e);
    }
}
