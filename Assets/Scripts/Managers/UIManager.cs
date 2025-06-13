using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    // sort order
    int order = 10;

    // 가장 마지막에 띄운 팝업이 가장 먼저 삭제되어야 하므로 스택 사용
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

    // 외부에서 팝업이 켜질 때, sort order 조정
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponenet<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;                      // 캔버스가 중첩되어 있을 때 부모와 독립적인 sorting order를 가짐

        if (sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        // 팝업과 연관이 없는 일반 UI
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

    // _name : 프리팹의 이름, T : 스크립트
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
