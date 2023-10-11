using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;



// 플레이어 기사 - 방패로 탄막을 튕겨냄
public class Knight : Player
{
    [SerializeField] private Transform shieldTransform;

    // 초기화 함수
    protected override void Init()
    {
        stats = new Stats(10, 2, 15f);
    }


    // // 공격을 받는 함수
    // public override void BeAttacked(int damage)
    // {
    //     if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

    //     // 멀티 : 각 클라이언트의 자신의 체력을 동기화
    //     if (GameManager.Instance.IsMultiPlay)
    //     {
    //         photonView.RPC("BeAttackedRPC", RpcTarget.All, damage);
    //     }
    //     // 싱글 : 체력만 소모
    //     else
    //     {
    //         stats.Health -= damage;
    //     }

    //     // 체력 UI 갱신
    //     GameManager.Instance.PlayerStatsUI.DecreasePlayerHp();

    //     // 체력이 0이 되면 엔딩 씬으로 이동
    //     if (stats.Health <= 0)
    //     {
    //         // 싱글 : 엔딩 씬으로 넘기기
    //         if (GameManager.Instance.IsMultiPlay == false)
    //         {
    //             gameObject.SetActive(false);
    //             GameManager.Instance.SceneMove(Define.ENDING_SCENE_NAME);
    //         }
    //         // 멀티 : 
    //         // 1. 플레이어 꺼주기
    //         // 2. 플레이어 생존수 감소
    //         // 3. 관전으로 변경
    //         // 4. 생존자 수가 0명 이하일 경우 엔딩으로 넘기기
    //         else if (GameManager.Instance.IsMultiPlay)
    //         {
    //             photonView.RPC("DiePlayerRPC", RpcTarget.All);
    //             foreach (KeyValuePair<int, GameObject> obj in GameManager.Instance.PlayerDictionary)
    //             {
    //                 // 자신과 다른 viewID 값 -> 다른 플레이어를 찾았을 때
    //                 if (photonView.ViewID != obj.Key)
    //                 {
    //                     GameManager.Instance.SwitchPlayerFollowingCamera(obj.Key);
    //                 }
    //             }
    //         }
    //     }
    // }

    // [PunRPC]
    // private void DiePlayerRPC()
    // {
    //     gameObject.SetActive(false);
    //     GameManager.Instance.ReductionPlayerLiveCount();
    //     if (GameManager.Instance.PlayerLiveCount <= 0)
    //     {
    //         SceneManager.LoadSceneAsync(Define.ENDING_SCENE_NAME);
    //     }
    // }

    // [PunRPC]
    // private void BeAttackedRPC(int damage)
    // {
    //     stats.Health -= damage;
    // }
}
