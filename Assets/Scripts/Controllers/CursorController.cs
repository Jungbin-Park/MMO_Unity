using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    Texture2D cursorTex_Atk;
    Texture2D cursorTex_Hand;

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    CursorType cursorType = CursorType.None;

    void Start()
    {
        // 커서 텍스처 로드
        cursorTex_Atk = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        cursorTex_Hand = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    void Update()
    {
        // 클릭을 하고 있는 상태면 커서를 바꾸지 않음
        if (Input.GetMouseButton(1))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(cursorTex_Atk, new Vector2(cursorTex_Atk.width / 5, 0), CursorMode.Auto);
                    cursorType = CursorType.Attack;
                }

            }
            else
            {
                if (cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(cursorTex_Hand, new Vector2(cursorTex_Hand.width / 3, 0), CursorMode.Auto);
                    cursorType = CursorType.Hand;
                }

            }
        }
    }
}
