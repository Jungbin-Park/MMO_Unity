using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Util
{
    // ��ɼ� �Լ�


    // recursive : ��������� ��� �ڽĵ��� ��ȸ�� ������
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
                if(string.IsNullOrEmpty(name) | component.name == name)
                    return component;
            }
        }
        // ���� �ڽĸ� ��ȸ
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
