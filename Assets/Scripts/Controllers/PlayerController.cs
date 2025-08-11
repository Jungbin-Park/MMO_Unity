using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public class PlayerController : BaseController
{
    int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
    //LayerMask mask = LayerMask.GetMask("Monster");
    //int mask = (1 << 9) | (1 << 8);

    PlayerStat stat;

    bool stopSkill = false;

    float attackSpeed = 0.5f;
    Animator anim;

    public override void Init()
    {
        stat = gameObject.GetComponent<PlayerStat>();

        // InputManager에서 어떤 키가 눌렸는지 추가로 함수 등록 요청(이벤트 등록 요청)
        //Managers.Input.KeyAction -= OnKeyboard; // 다른 곳에서 호출하면 안 되고 여기서만 추가되는 코드와 같으므로 주석 처리
        //Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        anim = GetComponent<Animator>();
        anim.SetFloat("AtkSpeed", attackSpeed);

        // HPBar 생성
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMove()
    {
        // =====
        // 공격
        // =====
        // 몬스터가 내 사정거리보다 가까우면 공격
        if(lockTarget != null)
        {
            destPos = lockTarget.transform.position;
            float distance = (destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                State = Define.State.SKILL;
                return;
            }
        }

        // =====
        // 이동
        // =====
        Vector3 dir = destPos - transform.position;
        // 목적지 도착
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.IDLE;
        }
        // 이동
        else
        {
            // 1. NavMesh 이동
            NavMeshAgent nma = gameObject.GetOrAddComponenet<NavMeshAgent>();
            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            // 앞에 벽이 있으면 Wall에 부딪히면 멈춤
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Wall")))
            {
                // 마우스를 누르고 있는 상태가 아니면 다시 멈춤 상태로
                if(!Input.GetMouseButton(1))
                    State = Define.State.IDLE;
                return;
            }

            // 캐릭터가 바라보는 방향 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

            // 2. 직접 이동
            /*
            // 캐릭터 이동
            float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            // 캐릭터가 바라보는 방향 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
            */
        }

    }

    protected override void UpdateSkill()
    {
        if(lockTarget != null)
        {
            Vector3 dir = lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }


        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Attack") && state.normalizedTime >= attackSpeed)
        {
            if (stopSkill)
            {
                State = Define.State.IDLE;
            }
            else
            {
                State = Define.State.SKILL;
            }
        }
        
    }

    void OnHitEvent()
    {
        if(lockTarget != null)
        {
            Stat targetStat = lockTarget.GetComponent<Stat>();
            PlayerStat myStat = gameObject.GetComponent<PlayerStat>();
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            Debug.Log(damage);
            targetStat.HP -= damage;
        }

    }

    //void OnKeyboard()
    //{
    //    if(Input.GetKey(KeyCode.W))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
    //        transform.position += Vector3.forward * Time.deltaTime * speed;
    //    }

    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
    //        transform.position += Vector3.back * Time.deltaTime * speed;
    //    }

    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
    //        transform.position += Vector3.left * Time.deltaTime * speed;
    //    }

    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
    //        transform.position += Vector3.right * Time.deltaTime * speed;
    //    }

    //    moveToDest = false;
    //}

    
    void OnMouseEvent(Define.MouseEvent _evt)
    {
        switch(State)
        {
            case Define.State.IDLE:
                OnMouseEvent_IdleMove(_evt);
                break;
            case Define.State.MOVE:
                OnMouseEvent_IdleMove(_evt);
                break;
            case Define.State.SKILL:
                {
                    if (_evt == Define.MouseEvent.PointerUp)
                        stopSkill = true;
                }
                break;
        }
    }

    void OnMouseEvent_IdleMove(Define.MouseEvent _evt)
    {
        // 카메라 -> 클릭 지점 레이캐스트
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, mask);

        switch (_evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        destPos = hit.point;
                        State = Define.State.MOVE;
                        stopSkill = false;  

                        // 몬스터를 클릭했을 때
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {
                            lockTarget = hit.collider.gameObject;
                        }
                        // 땅을 클릭했을 때 
                        else
                        {
                            lockTarget = null;
                        }
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (lockTarget == null && raycastHit)
                        destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                stopSkill = true;
                break;
        }
    }
}
