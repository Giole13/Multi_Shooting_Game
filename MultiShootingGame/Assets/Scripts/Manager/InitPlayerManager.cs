using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InitPlayerManager : MonoBehaviourPun
{
    [SerializeField] private GameObject Player;

    [SerializeField] private CameraController cameraController;

    // 멀티플레이어일 경우 플레이어 따로따로 스폰
    private void Awake()
    {
        // 싱글플레이일 경우 플레이어 스폰후 리턴
        if (GameManager.Instance.IsMultiPlay == false)
        {
            GameObject obj = Instantiate(Player);
            obj.GetComponent<PhotonRigidbody2DView>().enabled = false;
            obj.GetComponent<PhotonTransformView>().enabled = false;
            cameraController.SetCamera(obj.transform);
            return;
        }

        // 플레이어 생성
        GameObject multiPlayerObj = PhotonNetwork.Instantiate("Player_Knight", Vector2.zero, Quaternion.identity);

        // // 플레이어의 포톤 뷰를 가져온다.
        PhotonView playerPhotonView;
        multiPlayerObj.TryGetComponent<PhotonView>(out playerPhotonView);

        // Debug.Log(playerPhotonView.ViewID);

        // 몬스터에서 플레이어를 참조할 딕셔너리에 플레이어의 ViewID값으로 추가
        // GameManager.Instance.PlayerDictionary.Add(playerPhotonView.ViewID, multiPlayerObj);

        // 로컬의 객체에만 카메라 고정 적용
        if (playerPhotonView.IsMine)
        {
            cameraController.SetCamera(multiPlayerObj.transform);
        }


        // StartCoroutine(SetPlayerDic());
        // 플레이어 딕셔너리를 초기화하는 코루틴
        // IEnumerator SetPlayerDic()
        // {
        //     // 3초후 모든 플레이어를 참조해 딕셔너리에 추가
        //     yield return new WaitForSeconds(2f);
        //     // 모든 플레이어를 태그로 찾아 순차적으로 진행
        //     foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        //     {
        //         // 태그중 PhotonView가 없으면 패스
        //         // 찾은 플레이어에게서 포톤뷰를 참조해 딕셔너리에 추가
        //         if (playerObject.TryGetComponent<PhotonView>(out playerPhotonView))
        //         {
        //             GameManager.Instance.PlayerDictionary.Add(playerPhotonView.ViewID, playerObject);
        //         }
        //     }
        // }
    }

}
