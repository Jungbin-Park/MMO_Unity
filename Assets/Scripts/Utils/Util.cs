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

    // ���� ������Ʈ ���� FindChild
    // recursive : ��������� ��� �ڽĵ��� ��ȸ�� ������
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        // ��� ���� ������Ʈ�� Transform ������Ʈ�� ������ �����Ƿ� Transform Ȱ��
        Transform tr = FindChild<Transform>(go, name, recursive);

        if (tr == null)
            return null;

        return tr.gameObject;
    }
    
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if(go == null)
            return null;

        // ��� �ڽ��� ��ȸ(�ڽ��� �ڽı���)
        if (recursive)
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                // �̸��� null �̰ų�, ã�� �̸��� �ִ� ��� ��ȯ
                if(string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        // ���� �ڽĸ� ��ȸ
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
