using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// 플레이어의 이동에 관해 처리합니다.
public class PlayerMovement : MonoBehaviour
{
    Player player;

    private Vector2 inputVector;
    private Rigidbody2D playerRigid;

    private void Start()
    {
        player = GameManager.Instance.Player;
        playerRigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVector * player.stats.Speed * Time.fixedDeltaTime;
        playerRigid.MovePosition(playerRigid.position + nextVec);
    }

    private void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    private void OnDisable()
    {
        // Debug.Log("플레이어가 사라짐");
        // 플레이어가 사라지면 엔딩으로 넘어가기
        // GameManager.Instance.SceneMove(Define.ENDING_SCENE_NAME);
    }
}
