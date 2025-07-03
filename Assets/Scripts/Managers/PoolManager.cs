using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Resource Manager를 보조하는 역할
public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> poolStack = new Stack<Poolable>();

        public void Init(GameObject _original, int _count = 5)
        {
            Original = _original;
            Root = new GameObject().transform;
            Root.name = $"{Original.name}_Root";

            for(int i = 0; i < _count; i++)
            {
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponenet<Poolable>();
        }

        public void Push(Poolable _poolable)
        {
            if (_poolable == null)
                return;

            _poolable.transform.parent = Root;
            _poolable.gameObject.SetActive(false);
            _poolable.IsUsing = false;

            poolStack.Push(_poolable);
        }

        public Poolable Pop(Transform _parent)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            // DontDestroyOnLoad 해제 용도
            if (_parent == null)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = _parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> pool = new Dictionary<string, Pool>();
    Transform root;
    
    public void Init()
    {
        if(root == null)
        {
            root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(root);
        }
    }

    public void CreatePool(GameObject _original, int _count = 5)
    {
        Pool p = new Pool();
        p.Init(_original, _count);
        p.Root.parent = root;

        pool.Add(_original.name, p);
    }

    public void Push(Poolable _poolable)
    {
        string name = _poolable.gameObject.name;
        if(pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(_poolable.gameObject);
            return;
        }

        pool[name].Push(_poolable);
    }

    public Poolable Pop(GameObject _original, Transform _parent = null)
    {
        if (pool.ContainsKey(_original.name) == false)
            CreatePool(_original);

        return pool[_original.name].Pop(_parent);
    }

    public GameObject GetOriginal(string _name)
    {
        if (pool.ContainsKey(_name) == false)
            return null;

        return pool[_name].Original;
    }

    public void Clear()
    {
        foreach(Transform child in root)
        {
            GameObject.Destroy(child.gameObject);
        }

        pool.Clear();
    }
}
