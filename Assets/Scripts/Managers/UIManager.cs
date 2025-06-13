using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    // sort order
    int order = 10;

    // ���� �������� ��� �˾��� ���� ���� �����Ǿ�� �ϹǷ� ���� ���
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    UI_Scene sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    // �ܺο��� �˾��� ���� ��, sort order ����
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponenet<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;                      // ĵ������ ��ø�Ǿ� ���� �� �θ�� �������� sorting order�� ����

        if (sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        // �˾��� ������ ���� �Ϲ� UI
        else
        {
            canvas.sortingOrder = 0;
        }

    }

    public T MakeSubItem<T>(Transform parent = null, string _name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(_name))
            _name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{_name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponenet<T>(go);
    }

    public T ShowSceneUI<T>(string _name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(_name))
            _name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{_name}");
        T scene = Util.GetOrAddComponenet<T>(go);
        sceneUI = scene;

        go.transform.SetParent(Root.transform);

        return scene;
    }

    // _name : �������� �̸�, T : ��ũ��Ʈ
    public T ShowPopupUI<T>(string _name = null) where T : UI_Popup
    {
        if(string.IsNullOrEmpty(_name))
            _name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{_name}");
        T popup = Util.GetOrAddComponenet<T>(go);
        popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI(UI_Popup _popup)
    {
        if (popupStack.Count == 0) return;

        if(popupStack.Peek() != _popup)
        {
            Debug.Log("Failed to close Popup");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if(popupStack.Count == 0) return;

        UI_Popup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        order--;
    }

    public void CloseAllPopupUI()
    {
        while(popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        sceneUI = null;
    }
}
