using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionChecker : BaseManager<ConditionChecker>
{
    private List<IConditionOwner> _owners = new List<IConditionOwner>();

    public void Register(IConditionOwner owner) => _owners.Add(owner);
    public void Unregister(IConditionOwner owner) => _owners.Remove(owner);

    public void Check()
    {
        var snapshot = GameManager.Instance.TakeSnapshot();
        foreach (var owner in _owners)
        {
            if (owner.RequireAll.All(c => c.IsMet(snapshot)))
                owner.OnConditionMet();
            foreach (var c in owner.RequireAny)
            {
                bool result = c.IsMet(snapshot);
                if (result) c.OnMet?.Invoke();
                else c.OnNotMet?.Invoke();
            }
        }
    }
}
