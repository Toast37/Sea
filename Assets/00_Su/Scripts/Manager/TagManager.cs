using System.Collections.Generic;
using System.Linq;

public class TagManager
{
    private HashSet<string> _tags = new HashSet<string>();

    public void Add(string tag)
    {
        _tags.Add(tag);
        GameManager.Instance.NotifyStateChanged();
    }

    public void Remove(string tag)
    {
        _tags.Remove(tag);
        GameManager.Instance.NotifyStateChanged();
    }

    public bool Has(string tag) => _tags.Contains(tag);
    public bool HasAll(string[] tags) => tags.All(t => _tags.Contains(t));
}
