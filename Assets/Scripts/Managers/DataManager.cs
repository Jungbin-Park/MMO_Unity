using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int ,Stat> statDict { get; private set; } = new Dictionary<int ,Stat>();

    public void Init()
    {
        statDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string _path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{_path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
