using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;


// 플레이어의 입력에 관해 처리합니다.
public class PlayerInputHandler : MonoBehaviourPun
{
    #region 외부참조가능
    public Vector2 bulletDir { get; private set; }
    public Vector2 inputVector { get; private set; }

    #endregion 외부참조가능


    #region 외부참조불가능
    private Vector2 mousePosition;

    #endregion 외부참조불가능

    // 켜지면 PlayerInput 켜주기
    private void OnEnable()
    {
        GetComponent<PlayerInput>().enabled = true;
    }

    private void Update()
    {
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

        // 총구의 방향을 계산하는 로직
        bulletDir = (Camera.main.ScreenToWorldPoint(mousePosition) - transform.position).normalized;
    }

    private void OnAim(InputValue value)
    {
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

        mousePosition = value.Get<Vector2>();
    }

    private void OnMove(InputValue value)
    {
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

        inputVector = value.Get<Vector2>();
    }
}
