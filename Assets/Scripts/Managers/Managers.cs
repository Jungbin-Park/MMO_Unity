using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour 
{
    static Managers instance;
    public static Managers GetInst() { Init(); return instance; }

    private void Start()
    {
        Init();

        
    }

    private void Update()
    {
        
    }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null )
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();
        }


    }
}
