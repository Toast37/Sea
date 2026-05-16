using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Panel
{
    public string          Id;
    public List<BasePanel> Members;
    [NonSerialized] public Action<object> OnShow;
    [NonSerialized] public Action         OnHide;
}

public class UIManager : BaseManager<UIManager>
{
    [SerializeField] private List<Panel> _panels;

    private Dictionary<string, Panel> _registry;

    protected override void Awake()
    {
        base.Awake();
        _registry = new Dictionary<string, Panel>();
        foreach (var panel in _panels)
            if (!string.IsNullOrEmpty(panel.Id))
                _registry[panel.Id] = panel;
    }

    public Panel GetPanel(string id) =>
        _registry.TryGetValue(id, out var panel) ? panel : null;

    public void Register(string id, BasePanel member)
    {
        if (!_registry.TryGetValue(id, out var panel))
        {
            panel = new Panel { Id = id, Members = new List<BasePanel>() };
            _panels.Add(panel);
            _registry[id] = panel;
        }
        panel.Members.Add(member);
    }

    public void Unregister(string id, BasePanel member)
    {
        if (_registry.TryGetValue(id, out var panel))
            panel.Members.Remove(member);
    }

    public void Show(string id, object data = null)
    {
        if (!_registry.TryGetValue(id, out var panel)) return;
        foreach (var bp in panel.Members)
            if (bp != null) bp.Show(data);
        panel.OnShow?.Invoke(data);
    }

    public void Hide(string id)
    {
        if (!_registry.TryGetValue(id, out var panel)) return;
        foreach (var bp in panel.Members)
            if (bp != null) bp.Hide();
        panel.OnHide?.Invoke();
    }

    public void Toggle(string id, object data = null)
    {
        if (!_registry.TryGetValue(id, out var panel) || panel.Members.Count == 0) return;
        bool willShow = !panel.Members[0].gameObject.activeSelf;
        if (willShow) Show(id, data);
        else          Hide(id);
    }
}
