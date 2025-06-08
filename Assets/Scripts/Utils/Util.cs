using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponenet<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    // 게임 오브젝트 전용 FindChild
    // recursive : 재귀적으로 모든 자식들을 순회할 것인지
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        // 모든 게임 오브젝트는 Transform 컴포넌트를 가지고 있으므로 Transform 활용
        Transform tr = FindChild<Transform>(go, name, recursive);

        if (tr == null)
            return null;

        return tr.gameObject;
    }
    
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if(go == null)
            return null;

        // 모든 자식을 순회(자식의 자식까지)
        if (recursive)
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                // 이름이 null 이거나, 찾는 이름이 있는 경우 반환
                if(string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        // 직속 자식만 순회
        else
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if(component != null)
                        return component;
                }
            }
        }

        return null;
    }
}
