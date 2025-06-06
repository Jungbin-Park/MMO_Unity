using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    // ������Ʈ Ÿ�� �� �迭�� ������Ʈ ����
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

    // ���÷��� ���
    void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // ���� Enum�� ��� ���� �̸��� ����
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] arrObject = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), arrObject);

        for(int i = 0; i < names.Length; ++i)
        {
            // ���� ������Ʈ�� �ڽĵ��� ��ȸ�ϸ� ���� �̸����� ã��
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
