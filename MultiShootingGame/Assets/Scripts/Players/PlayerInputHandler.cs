using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// 플레이어의 입력에 관해 처리합니다.
public class PlayerInputHandler : MonoBehaviour
{
    #region 외부참조가능
    public Vector2 bulletDir { get; private set; }
    public Vector2 inputVector { get; private set; }

    #endregion 외부참조가능


    #region 외부참조불가능
    private Vector2 mousePosition;

    #endregion 외부참조불가능

    private void Update()
    {
        // 총구의 방향을 계산하는 로직
        bulletDir = (Camera.main.ScreenToWorldPoint(mousePosition) - transform.position).normalized;

    }

    private void OnAim(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }

    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
}
