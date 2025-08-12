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

        // 플레이어, 몬스터 스폰
        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Hero");
        Managers.Game.Spawn(Define.WorldObject.Monster, "Skeleton");
        // 카메라에 플레이어 연결
        Camera.main.gameObject.GetOrAddComponenet<CameraController>().SetTarget(player);
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
