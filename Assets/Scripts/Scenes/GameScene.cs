using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Managers.UI.ShowSceneUI<UI_Inven>();

        Dictionary<int, Data.Stat> dict = Managers.Data.statDict;

        gameObject.GetOrAddComponenet<CursorController>();
    }

    IEnumerator ExplodeAfterSeconds(float seconds)
    {
        Debug.Log("co Start");
        yield return new WaitForSeconds(seconds);
        Debug.Log("co end");
    }

    public override void Clear()
    {

    }
}
