using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    Stat stat;

    public override void Init()
    {
        stat = gameObject.GetComponent<Stat>();

        // HPBar »ý¼º
        if(gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle()
    {
        Debug.Log("UpdateIdle");
    }

    protected override void UpdateMove()
    {
        Debug.Log("UpdateMove");
    }

    protected override void UpdateSkill()
    {
        Debug.Log("UpdateSkill");
    }

    protected override void UpdateDead()
    {
        
    }

    void OnHitEvent()
    {
        Debug.Log("Monster OnHitEvent");
    }
}
