using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.Scene _type)
    {
        CurrentScene.Clear();
        SceneManager.LoadScene(GetSceneName(_type));
    }

    string GetSceneName(Define.Scene _type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), _type);
        return name;
    }


}
