using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat stat;

    [SerializeField]
    float scanRange = 10.0f;

    [SerializeField]
    float attackRange = 2.0f;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;

        stat = gameObject.GetComponent<Stat>();

        // HPBar ����
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle()
    {
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;

        float distance = (player.transform.position - transform.position).magnitude;
        if(distance < scanRange)
        {
            lockTarget = player;

            State = Define.State.MOVE;

            return;
        }
    }

    protected override void UpdateMove()
    {
        // =====
        // 공격
        // =====
        // 플레이어가 내 사정거리보다 가까우면 공격
        if (lockTarget != null)
        {
            destPos = lockTarget.transform.position;
            float distance = (destPos - transform.position).magnitude;
            if (distance <= attackRange)
            {
                NavMeshAgent nma = gameObject.GetOrAddComponenet<NavMeshAgent>();
                nma.SetDestination(transform.position);

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
            // NavMesh 이동
            NavMeshAgent nma = gameObject.GetOrAddComponenet<NavMeshAgent>();
            nma.SetDestination(destPos);
            nma.speed = stat.MoveSpeed;

            // 캐릭터가 바라보는 방향 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

        }
    }

    protected override void UpdateSkill()
    {
        // 타겟을 향해 부드러운 회전
        if (lockTarget != null)
        {
            Vector3 dir = lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    protected override void UpdateDead()
    {
        
    }

    void OnHitEvent()
    {
        if(lockTarget != null)
        {
            // 공격
            Stat targetStat = lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(stat);

            if(targetStat.HP <= 0)
            {
                State = Define.State.IDLE;
            }
            else
            {
                float distance = (lockTarget.transform.position - transform.position).magnitude;
                if (distance <= attackRange)
                    State = Define.State.SKILL;
                else
                    State = Define.State.MOVE;
            }
        }
        else
        {
            State = Define.State.IDLE;
        }
    }
}
