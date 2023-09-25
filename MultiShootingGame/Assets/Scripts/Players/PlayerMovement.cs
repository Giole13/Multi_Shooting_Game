using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


// 플레이어의 이동에 관해 처리합니다.
public class PlayerMovement : MonoBehaviourPun
{
    private Player player;
    private Rigidbody2D playerRigid;
    private PlayerInputHandler playerInputHandler;

    private void Start()
    {
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

        player = GameManager.Instance.Player;
        TryGetComponent<Rigidbody2D>(out playerRigid);
        TryGetComponent<PlayerInputHandler>(out playerInputHandler);
    }

    private void FixedUpdate()
    {
        // 자신만 식별
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

        Vector2 nextVec = playerInputHandler.inputVector * player.stats.Speed * Time.fixedDeltaTime;
        playerRigid.MovePosition(playerRigid.position + nextVec);
    }

}
