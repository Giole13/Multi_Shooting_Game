using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.Assertions;

// 게임의 시스템을 책임지는 클래스
public class GameManager : Singleton<GameManager>
{
    #region 플레이어 스탯 UI 스크립트
    private PlayerStatsUI playerStatsUI = null;
    public PlayerStatsUI PlayerStatsUI
    {
        get
        {
            if (playerStatsUI is null)
            {
                playerStatsUI = GameObject.Find("PlayerUI").GetComponent<PlayerStatsUI>();
                return playerStatsUI;
            }
            return playerStatsUI;
        }
    }
    #endregion 플레이어 스탯 UI 스크립트

    #region  플레이어 스크립트
    private Player player = null;
    public Player Player
    {
        get
        {
            if (player is null)
            {
                player = PlayerTransform.GetComponent<Player>();
                return player;
            }
            return player;
        }
    }
    #endregion  플레이어 스크립트

    #region 플레이어 트랜스폼
    private Transform playerTransform = null;
    public Transform PlayerTransform
    {
        get
        {
            if (playerTransform is null)
            {
                playerTransform = FindObjectOfType<Player>().transform;
                return playerTransform;
            }
            return playerTransform;
        }
    }
    #endregion 플레이어 트랜스폼

    #region 멀티 : 플레이어를 구분할 딕셔너리
    public Dictionary<int, GameObject> PlayerDictionary = null;
    #endregion 멀티 : 플레이어를 구분할 딕셔너리

    #region 멀티 : 플레이어의 생존한 사람의 수를 나타내는 정수
    public int PlayerLiveCount
    {
        get;
        private set;
    }
    #endregion 멀티 : 플레이어의 생존한 사람의 수를 나타내는 정수


    // 싱글, 멀티 구분 bool 타입
    public bool IsMultiPlay { get; private set; } = false;


    private Transform inGameCameraTransform;
    private bool IsCameraFollowingPlayer = false;

    //  게임이 끝나고 전부 초기화 해야해주는 함수
    public override void Init()
    {
        playerStatsUI = null;
        player = null;
        playerTransform = null;
        PlayerDictionary = new Dictionary<int, GameObject>();
    }

    // 시작할 때 모두 초기화 해주기
    private void Awake()
    {
        Instance.Init();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (IsCameraFollowingPlayer)
        {
            // 설정한 타겟을 카메라는 계속 따라간다.
            inGameCameraTransform.position = PlayerTransform.position + new Vector3(0f, 0f, -10f);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 이름에 따라 초기화 하는 것들을 다르게 해주기
        switch (scene.name)
        {
            case Define.TITLE_SCENE_NAME:
                break;
            case Define.INGAME_SCENE_NAME:
                InitInGame();
                break;
            case Define.ENDING_SCENE_NAME:
                InitEndingGame();
                break;
        }
    }

    private void InitEndingGame()
    {
        // 만약 멀티플레이라면 연결 끊기
        if (Instance.IsMultiPlay)
        {
            PhotonNetwork.Disconnect();
        }
        // 객체 초기화
        Instance.Init();
        IsCameraFollowingPlayer = false;
    }

    // 인게임 초기화 하는 함수
    private void InitInGame()
    {
        inGameCameraTransform = GameObject.FindWithTag("MainCamera").transform;

        // 싱글플레이일 경우 플레이어 스폰후 리턴
        if (GameManager.Instance.IsMultiPlay == false)
        {
            IsCameraFollowingPlayer = true;
            GameObject obj = Instantiate(Resources.Load("Player_Knight") as GameObject);
            playerTransform = obj.transform;
            obj.GetComponent<PhotonRigidbody2DView>().enabled = false;
            obj.GetComponent<PhotonTransformView>().enabled = false;
            return;
        }

        // 플레이어 생성
        GameObject multiPlayerObj = PhotonNetwork.Instantiate("Player_Knight", Vector2.zero, Quaternion.identity);

        // // 플레이어의 포톤 뷰를 가져온다.
        PhotonView playerPhotonView;
        multiPlayerObj.TryGetComponent<PhotonView>(out playerPhotonView);

        // 로컬의 객체에만 카메라 고정 적용
        if (playerPhotonView.IsMine)
        {
            playerTransform = multiPlayerObj.transform;
            IsCameraFollowingPlayer = true;
        }
    }

    // 멀티 : 플레이어가 죽어서 다른 사람을 관전하는 함수
    public void SwitchPlayerFollowingCamera(int viewID)
    {
        Debug.Assert(PlayerDictionary[viewID] != null);
        Debug.Assert(PlayerDictionary[viewID].transform != null);
        playerTransform = PlayerDictionary[viewID].transform;
    }

    // 멀티 플레이 설정하는 함수
    public void SettingMultiPlay(bool IsMulti)
    {
        IsMultiPlay = IsMulti;
        PlayerLiveCount = 2;
    }

    // 플레이어가 죽으면 생존 카운트를 내리고 모든 플레이어가 죽으면 true를 반환하는 함수
    public void ReductionPlayerLiveCount()
    {
        PlayerLiveCount--;
        if (GameManager.Instance.PlayerLiveCount <= 0)
        {
            SceneManager.LoadSceneAsync(Define.ENDING_SCENE_NAME);
        }
    }

    /// <summary>씬을 이동하는 함수</summary>
    /// <param name="SceneName">이동할 씬 이름</param>
    public void SceneMove(string SceneName)
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }


    // 데스크톱으로 나가는 함수
    public void ExitDesktop()
    {
#if UNITY_EDITOR
        // Unity의 플레이 버튼을 꺼주는 함수
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 앱을 종료해주는 함수
        Application.Quit();
#endif
    }


}
