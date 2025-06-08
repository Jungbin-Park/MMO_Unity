using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    // ������Ʈ Ÿ�� �� �迭�� ������Ʈ ����
    protected Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();

    // ���÷��� ���
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // ���� Enum�� ��� ���� �̸��� ����
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] arrObject = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), arrObject);

        // ���� ������Ʈ�� �ڽĵ��� ��ȸ�ϸ� ���� �̸����� ã��
        for (int i = 0; i < names.Length; ++i)
        {
            // ���� ������Ʈ ����
            if (typeof(T) == typeof(GameObject))
                arrObject[i] = Util.FindChild(gameObject, names[i], true);
            // ������Ʈ ����
            else
                arrObject[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (arrObject[i] == null)
            {
                Debug.Log($"Error : Failed to bind ({names[i]})");
            }
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objs = null;
        if (objects.TryGetValue(typeof(T), out objs) == false)
        {
            return null;
        }

        return objs[idx] as T;
    }

    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponenet<UI_EventHandler>(go);

        switch(type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
