using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform followingTarget;

    private void Update()
    {
        // 설정한 타겟을 카메라는 계속 따라간다.
        transform.position = followingTarget.position + new Vector3(0f, 0f, -10f);
    }

    // 설정한 타켓으로 카메라를 고정한다.
    public void SetCamera(Transform target)
    {
        followingTarget = target;
    }
}
