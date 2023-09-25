using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InitPlayerManager : MonoBehaviourPun
{
    [SerializeField] private GameObject Player;

    // 멀티플레이어일 경우 플레이어 따로따로 스폰
    private void Awake()
    {
        // 싱글플레이일 경우 플레이어 스폰후 리턴
        if (GameManager.Instance.IsMultiPlay == false)
        {
            Instantiate(Player);
            return;
        }

        // 플레이어 생성
        GameObject obj = PhotonNetwork.Instantiate("Player_Knight", Vector2.zero, Quaternion.identity);
        obj.GetComponent<PhotonRigidbody2DView>().enabled = true;
        obj.GetComponent<PhotonTransformView>().enabled = true;
    }
}
