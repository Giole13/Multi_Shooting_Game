using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;



// 엔딩에서 새로운 게임을 하기 위해 초기화 해주는 함수
public class GameSetManager : MonoBehaviourPun
{
    private void Awake()
    {
        if (GameManager.Instance.IsMultiPlay)
        {
            // 방 나가기
            PhotonNetwork.Disconnect();
        }

        GameManager.Instance.Init();
        LoadingManager.Instance.Init();
    }



}
