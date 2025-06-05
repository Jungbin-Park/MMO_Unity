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
        // 카메라 이동은 플레이어의 이동(Update)이 끝난 후에 해야 덜덜거림이 사라짐

        if (mode == Define.CameraMode.QuarterView)
        {
            // 플레이어 -> 카메라 레이캐스팅
            RaycastHit hit;

            // 플레이어와 카메라 사이에 벽이 있으면
            if(Physics.Raycast(player.transform.position, camPos, out hit, camPos.magnitude, LayerMask.GetMask("Wall")))
            {
                // 벽 앞으로 이동
                float dist = (hit.point - player.transform.position).magnitude - 0.8f;
                transform.position = player.transform.position + camPos.normalized * dist;
            }
            else
            {
                // 플레이어를 바라보는 카메라 이동
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
