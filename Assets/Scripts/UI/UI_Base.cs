using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    // 오브젝트 타입 별 배열로 오브젝트 관리
    protected Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();

    // 리플렉션 사용
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // 현재 Enum의 모든 값의 이름을 받음
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] arrObject = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), arrObject);

        // 현재 오브젝트의 자식들을 순회하며 같은 이름인지 찾음
        for (int i = 0; i < names.Length; ++i)
        {
            // 게임 오브젝트 전용
            if (typeof(T) == typeof(GameObject))
                arrObject[i] = Util.FindChild(gameObject, names[i], true);
            // 컴포넌트 전용
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
