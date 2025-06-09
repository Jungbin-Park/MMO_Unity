using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0f;

    Vector3 destPos;

    void Start()
    {
        // InputManager���� � Ű�� ������ �߰��� �Լ� �����û(�̺�Ʈ ���� ��û)
        //Managers.Input.KeyAction -= OnKeyboard; // �ٸ� ������ ȣ���� �� ��� �� �� �߰��Ǵ� �Ϳ� ����ؼ� ���� ����
        //Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    public enum PlayerState
    {
        IDLE,
        MOVE,
        DEAD,
    }

    PlayerState state = PlayerState.IDLE;

    void UpdateDead()
    {

    }

    void UpdateMove()
    {
        Vector3 dir = destPos - transform.position;

        if (dir.magnitude < 0.0001f)
        {
            state = PlayerState.IDLE;
        }
        else
        {
            // �÷��̾� �̵�
            float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            // �ε巯�� �÷��̾� ȸ��
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        // �ִϸ��̼�
        Animator anim = GetComponent<Animator>();
        // ���� ���� ���¿� ���� ������ �Ѱ��ش�
        anim.SetFloat("speed", speed);
    }

    void UpdateIdle()
    {
        // �ִϸ��̼�
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    void Update()
    {
        switch(state)
        {
            case PlayerState.DEAD:
                UpdateDead();
                break;
            case PlayerState.MOVE:
                UpdateMove();
                break;
            case PlayerState.IDLE:
                UpdateIdle();
                break;
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

    void OnMouseClicked(Define.MouseEvent mouseEvent)
    {
        if (state == PlayerState.DEAD)
            return;

        // ī�޶� -> Ŭ�� ���� ����ĳ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        //LayerMask mask = LayerMask.GetMask("Monster");
        //int mask = (1 << 9) | (1 << 8);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            destPos = hit.point;
            state = PlayerState.MOVE;
        }
    }
}
