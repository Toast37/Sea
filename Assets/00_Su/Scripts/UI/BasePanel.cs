using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    public virtual void Show(object data)
    {
        gameObject.SetActive(true);
        OnReceive(data);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnReceive(object data) { }
}
