using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text serverInfoText;


    // 서버에 접속하는 함수
    public void SettingMultiPlayer()
    {
        // 서버에 접속하는 함수
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 연결 되었을때 호출되는 함수
    public override void OnConnectedToMaster()
    {
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
        serverInfoText.text = "방을 생성 완료!";
    }

    // 방에 들어왔을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        serverInfoText.text = "방에 입장했어요! 플레이어를 기다리는중...";
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
        yield return new WaitUntil(() => /*room.MaxPlayers*/ 1 == room.PlayerCount);
        serverInfoText.text = $"현재 방의 인원 : {room.PlayerCount}";

        yield return new WaitForSeconds(1f);
        serverInfoText.text = $"방에 입장합니다...";

        // 1초후 캐릭터 선택 씬으로 모든 플레이어 전환
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel(Define.LOADING_SCENE_NAME);
        PhotonNetwork.AutomaticallySyncScene = true;
    }




}