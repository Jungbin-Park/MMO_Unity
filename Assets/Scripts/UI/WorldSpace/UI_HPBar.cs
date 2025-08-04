using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar,
    }

    Stat stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        // hpbar를 소유하고 있는 오브젝트의 Transform
        Transform parent = transform.parent;
        // 오브젝트의 콜라이더 크기만큼 더해서 오브젝트 위에 위치시킴
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y) + (Vector3.up);
        // 카메라를 쳐다보도록
        transform.rotation = Camera.main.transform.rotation;

        // HP 비율 적용
        float ratio = stat.HP / (float)stat.MaxHp;
        SetHPRatio(ratio);
    }

    public void SetHPRatio(float _ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = _ratio;
    }
}
