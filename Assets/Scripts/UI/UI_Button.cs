using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    // 오브젝트 타입 별 배열로 오브젝트 관리
    Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();

    enum Buttons
    {
        PointButton,

    }

    enum Texts
    {
        Text_Button,
        Text_Score,
    }

    // 리플렉션 사용
    void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // 현재 Enum의 모든 값의 이름을 받음
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] arrObject = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), arrObject);

        for(int i = 0; i < names.Length; ++i)
        {
            // 현재 오브젝트의 자식들을 순회하며 같은 이름인지 찾음
            arrObject[i] = Util.FindChild<T>(gameObject, names[i], true);
        }
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    int score = 0;  

    public void OnButtonClicked()
    {
        score++;
    }
}
