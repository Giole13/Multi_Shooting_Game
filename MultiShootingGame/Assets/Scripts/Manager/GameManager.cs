using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
                playerTransform = GameObject.FindWithTag("Player").transform;
                return playerTransform;
            }
            return playerTransform;
        }
    }
    #endregion 플레이어 트랜스폼



    // 게임 시작시 모두 초기화
    private void Awake()
    {
        // SceneManager.sceneLoaded += PoolManagerOnEnable();
        // // 인게임에 들어온다면 오브젝트 풀링을 켜줘라
        // if (SceneManager.GetActiveScene().name == Define.INGAME_SCENE_NAME)
        // {
        // }
    }

    // private void PoolManagerOnEnable()
    // {
    //     PoolManager.Instance.gameObject.SetActive(true);

    // }


    // 싱글, 멀티 구분 bool 타입
    public bool IsMultiPlay { get; private set; } = false;

    // 멀티 플레이 설정하는 함수
    public void MultiPlaySetting()
    {
        IsMultiPlay = true;
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
