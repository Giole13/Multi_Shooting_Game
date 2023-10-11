using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text serverInfoText;

    [SerializeField] private TitleManager titleManager;

    [SerializeField] private Transform playerCountTransform;


    // 서버에 접속하는 함수
    public void SettingMultiPlayer()
    {
        serverInfoText.text = "서버 접속 시도중...";
        // 서버에 접속하는 함수
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 연결 되었을때 호출되는 함수
    public override void OnConnectedToMaster()
    {
        serverInfoText.text = "방에 입장중...";
        // 서버의 목록에서 방이 존재하는지 검색한다.
        PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤한 방에 접속을 실패하면 실행되는 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        serverInfoText.text = "방입장에 실패! 방을 생성합니다...";
        // 랜덤으로 방에 입장하거나 조건이 맞지않으면 방을 생성하는 함수
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    // 방을 생성되었을때 호출되는 함수
    public override void OnCreatedRoom()
    {
        serverInfoText.text = "방 생성 완료!";
    }

    // 방에 들어왔을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        serverInfoText.text = "플레이어를 기다리는중...";
        // 1. 플레이어 입장을 기다린다.
        // 2. 플레이어가 2명이상이 되면 캐릭터 선택 씬으로 넘어간다.
        StartCoroutine(WaitForPlayer());
    }

    // 플레이어를 기다리는 함수
    public IEnumerator WaitForPlayer()
    {
        Room room = PhotonNetwork.CurrentRoom;
        serverInfoText.text = $"현재 방의 인원 : {room.PlayerCount}";

        // 현재 플레이어가 MaxPlayers 면 실행
        // 디버그용 /*room.MaxPlayers*/
        yield return new WaitUntil(() => room.MaxPlayers == room.PlayerCount);
        serverInfoText.text = $"현재 방의 인원 : {room.PlayerCount}";

        yield return new WaitForSeconds(1f);
        serverInfoText.text = $"방에 입장합니다...";

        yield return new WaitForSeconds(1f);
        // 타이틀 화면에서 캐릭터 선택화면으로 바꿔주기만 하기
        titleManager.SwitchSelectCharacterScreen();

        photonView.RPC("CountingPlayerNum", RpcTarget.AllBuffered, room.PlayerCount);
    }

    // 플레이어가 몇명 들어왔는지 카운트 해주는 함수
    [PunRPC]
    private void CountingPlayerNum(int playerCount)
    {
        // 플레이어 표시하는 로직 초기화
        foreach (Transform obj in playerCountTransform)
        {
            obj.gameObject.SetActive(false);
        }

        // 플레이어 수만큼 표시해주기
        for (int i = 0; i < playerCount; i++)
        {
            playerCountTransform.GetChild(i).gameObject.SetActive(true);
        }
    }

    // 멀티 환경으로 게임 시작
    public void StartInGame()
    {
        // 초당 동기화 함수 호출 수 설정
        PhotonNetwork.SendRate = 60;

        photonView.RPC("ReadyGamePlayRPC", RpcTarget.All);
    }

    // 각 클라이언트에서 준비 완료를 갱신하기 위한 함수
    [PunRPC]
    private void ReadyGamePlayRPC()
    {
        // 만약 함수를 실행했는데 true면 (모든 플레이어가 준비가 되면)
        // 게임 시작
        if (titleManager.ReadyToGamePlay())
        {
            photonView.RPC("SceneMove", RpcTarget.All);
        }
    }

    [PunRPC]
    private void SceneMove()
    {
        // 5초 후 게임 시작
        StartCoroutine(StartGame());
        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadSceneAsync(Define.INGAME_SCENE_NAME);
        }
    }

    // 게임이 끝나면 서버를 나가는 함수 
    public void LeaveServer()
    {
        PhotonNetwork.LeaveRoom();
    }
}
