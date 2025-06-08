using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    // sort order
    int order = 0;

    // 가장 마지막에 띄운 팝업이 가장 먼저 삭제되어야 하므로 스택 사용
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();

    // _name : 프리팹의 이름, T : 스크립트
    public T ShowPopupUI<T>(string _name = null) where T : UI_Popup
    {
        if(string.IsNullOrEmpty(_name))
            _name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{_name}");

        T popup = Util.GetOrAddComponenet<T>(go);
        popupStack.Push(popup);


        return popup;
    }

    public void ClosePopupUI()
    {
        if(popupStack.Count == 0) return;

        UI_Popup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        order--;
    }
}
