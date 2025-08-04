using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool pressed = false;
    float pressedTime = 0.0f;

    public void OnUpdate()
    {
        // UI 버튼이 클릭되었는지
        if (EventSystem.current.IsPointerOverGameObject())
            return;


        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if(MouseAction != null)
        {
            if(Input.GetMouseButton(1))
            {
                if(!pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown); 
                    pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                pressed = true;
            }
            else
            {
                if (pressed)
                {
                    if(Time.time < pressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                    
                pressed = false;
                pressedTime = 0.0f;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
