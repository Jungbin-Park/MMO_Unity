using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Util
{
    // 기능성 함수


    // recursive : 재귀적으로 모든 자식들을 순회할 것인지
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
                if(string.IsNullOrEmpty(name) | component.name == name)
                    return component;
            }
        }
        // 직속 자식만 순회
        else
        {
            for(int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) | transform.name == name)
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
