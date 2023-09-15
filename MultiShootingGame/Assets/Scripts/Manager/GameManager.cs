using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// 게임의 시스템을 책임지는 클래스
public class GameManager : Singleton<GameManager>
{
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
