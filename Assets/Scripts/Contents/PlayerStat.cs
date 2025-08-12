using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int exp;
    [SerializeField]
    protected int gold;

    public int Exp 
    {  
        get { return exp; } 
        set 
        {  
            exp = value;

            // 레벨업 체크
            int level = Level;
            while(true)
            {
                Data.Stat stat;
                // 다음 레벨이 데이터에 없으면 중단
                if (Managers.Data.statDict.TryGetValue(level + 1, out stat) == false)
                    break;
                // 현재 경험치를 다음 레벨의 요구 경험치와 비교
                if (exp < stat.totalExp)
                    break;
                level++;
            }

            if(level != Level)
            {
                Debug.Log("level up!");
                Level = level;
                SetStat(level);
            }
        } 
    }
    public int Gold 
    { 
        get { return gold; } 
        set {  gold = value; } 
    }

    private void Start()
    {
        level = 1;

        exp = 0;
        gold = 0;

        SetStat(level);
    }

    public void SetStat(int _level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.statDict;

        Data.Stat stat = dict[_level];

        hp = stat.maxHp;
        maxHp = stat.maxHp;
        attack = stat.attack;
        defense = stat.defense;
        moveSpeed = stat.moveSpeed;
    }

    protected override void OnDead(Stat _attackerStat)
    {
        Debug.Log("Player OnDead");
    }
}
