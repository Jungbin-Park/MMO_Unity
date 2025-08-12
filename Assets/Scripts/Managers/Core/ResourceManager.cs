using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        // original을 들고 있으면 바로 사용
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int idx = name.LastIndexOf('/');
            if(idx > 0)
                name = name.Substring(idx + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if(go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        // 프리팹 원본
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 복사본
        // 1. 풀링되는 오브젝트면
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // 2. 풀링 대상이 아니면
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        // 풀링되는 오브젝트라면 -> PoolManager에게 위탁
        Poolable poolable = gameObject.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(gameObject);
    }
}
