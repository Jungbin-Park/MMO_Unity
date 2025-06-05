using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode mode = Define.CameraMode.QuarterView;
    [SerializeField]
    public Vector3 camPos = new Vector3(0.0f, 6.0f, -5.0f);
    [SerializeField]
    public GameObject player = null;
    
    void Start()
    {
        
    }

    void LateUpdate()
    {
        // ī�޶� �̵��� �÷��̾��� �̵�(Update)�� ���� �Ŀ� �ؾ� �����Ÿ��� �����

        if (mode == Define.CameraMode.QuarterView)
        {
            // �÷��̾� -> ī�޶� ����ĳ����
            RaycastHit hit;

            // �÷��̾�� ī�޶� ���̿� ���� ������
            if(Physics.Raycast(player.transform.position, camPos, out hit, camPos.magnitude, LayerMask.GetMask("Wall")))
            {
                // �� ������ �̵�
                float dist = (hit.point - player.transform.position).magnitude - 0.8f;
                transform.position = player.transform.position + camPos.normalized * dist;
            }
            else
            {
                // �÷��̾ �ٶ󺸴� ī�޶� �̵�
                transform.position = player.transform.position + camPos;
                transform.LookAt(player.transform);
            }

            
        }
        
    }

    public void SetQuarterView(Vector3 _camPos)
    {
        mode = Define.CameraMode.QuarterView;
        camPos = _camPos;
    }
}
