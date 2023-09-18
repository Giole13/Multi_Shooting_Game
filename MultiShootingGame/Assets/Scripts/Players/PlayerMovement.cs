using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어의 이동에 관해 처리합니다.
public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D playerRigid;
    private PlayerInputHandler playerInputHandler;

    private void Start()
    {
        player = GameManager.Instance.Player;
        TryGetComponent<Rigidbody2D>(out playerRigid);
        TryGetComponent<PlayerInputHandler>(out playerInputHandler);
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = playerInputHandler.inputVector * player.stats.Speed * Time.fixedDeltaTime;
        playerRigid.MovePosition(playerRigid.position + nextVec);
    }

    private void OnDisable()
    {
        // Debug.Log("플레이어가 사라짐");
        // 플레이어가 사라지면 엔딩으로 넘어가기
        // GameManager.Instance.SceneMove(Define.ENDING_SCENE_NAME);
    }

}
