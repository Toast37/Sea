using UnityEngine;

public class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _isQuitting = false;
    public static T Instance
    {
        get
        {
            if (_isQuitting)
            {
                return null;
            }
            if (_instance == null)
            {
                _instance = Object.FindAnyObjectByType<T>(FindObjectsInactive.Include);

                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name + " (Singleton)");
                    _instance = go.AddComponent<T>();
                    Debug.Log($"<color=red>警告！</color> 谁在重复创建: {UnityEngine.StackTraceUtility.ExtractStackTrace()}");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        _isQuitting=true;
    }
}