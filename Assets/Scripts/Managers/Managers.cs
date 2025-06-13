using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour 
{
    static Managers instance;
    static Managers GetInst() { Init(); return instance; }

    InputManager input = new InputManager();
    ResourceManager resource = new ResourceManager();
    SceneManagerEx scene = new SceneManagerEx();
    SoundManager sound = new SoundManager();
    UIManager ui = new UIManager();

    public static InputManager Input { get { return GetInst().input; } }
    public static ResourceManager Resource { get { return GetInst().resource; } }
    public static SceneManagerEx Scene { get { return GetInst().scene; } }
    public static SoundManager Sound { get { return GetInst().sound; } }
    public static UIManager UI { get { return GetInst().ui; } }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        input.OnUpdate();
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

            // SoundManager √ ±‚»≠
            instance.sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();
    }
}
