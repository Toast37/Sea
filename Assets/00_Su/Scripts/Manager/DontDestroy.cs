using UnityEngine;

/// <summary>挂在需要跨场景保留的根节点上（UIs、Player 等），使其不随场景销毁。</summary>
public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        // 场景重载时同名对象已存在，销毁新创建的副本
        var all = FindObjectsByType<DontDestroy>(FindObjectsSortMode.None);
        foreach (var obj in all)
        {
            if (obj != this && obj.name == gameObject.name)
            {
                Destroy(gameObject);
                return;
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}
