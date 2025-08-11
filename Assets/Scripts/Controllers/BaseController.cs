using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 destPos;

    [SerializeField]
    protected Define.State state = Define.State.IDLE;

    [SerializeField]
    protected GameObject lockTarget;

    public virtual Define.State State
    {
        get { return state; }
        set
        {
            state = value;

            Animator anim = GetComponent<Animator>();
            switch (state)
            {
                case Define.State.IDLE:
                    anim.CrossFade("Idle", 0.1f);
                    break;
                case Define.State.MOVE:
                    // 이동 속도에 따라 애니메이션 재생
                    //anim.SetFloat("speed", stat.MoveSpeed);
                    anim.CrossFade("Move", 0.1f);
                    break;
                case Define.State.SKILL:
                    anim.CrossFade("Attack", 0.1f, -1, 0.0f);
                    break;
                case Define.State.DEAD:
                    break;
            }
        }
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        switch (State)
        {
            case Define.State.DEAD:
                UpdateDead();
                break;
            case Define.State.MOVE:
                UpdateMove();
                break;
            case Define.State.IDLE:
                UpdateIdle();
                break;
            case Define.State.SKILL:
                UpdateSkill();
                break;
        }

    }

    public abstract void Init();
    protected virtual void UpdateDead() { }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
}
