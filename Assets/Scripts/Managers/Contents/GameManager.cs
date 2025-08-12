using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // 서버 연동을 하는 경우 ID - GameObject를 들고 있도록 함
    /*
    Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> monsters = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> env = new Dictionary<int, GameObject>();
    */

    GameObject player;
    HashSet<GameObject> monsters = new HashSet<GameObject>();

    public GameObject GetPlayer() { return player; }

    public GameObject Spawn(Define.WorldObject _type, string _path, Transform _parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(_path, _parent);

        switch(_type)
        {
            case Define.WorldObject.Monster:
                monsters.Add(go);
                break;
            case Define.WorldObject.Player:
                player = go;
                break;

        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject _go)
    {
        // 컨트롤러를 통해 플레이어인지 몬스터인지 구분한다
        BaseController bc = _go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.UnKnown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject _go)
    {
        Define.WorldObject type = GetWorldObjectType(_go);

        switch(type)
        {
            case Define.WorldObject.Monster:
                {
                    if(monsters.Contains(_go))
                        monsters.Remove(_go);
                }
                break;
            case Define.WorldObject.Player:
                {
                    if (player == _go)
                        player = null;
                }
                break;
        }

        Managers.Resource.Destroy(_go);
    }
}
