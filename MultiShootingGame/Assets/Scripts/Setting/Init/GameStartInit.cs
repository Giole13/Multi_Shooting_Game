using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 초기값들을 설정해주는 스크립트
public class GameStartInit : MonoBehaviour
{
    void Start()
    {
        // 타이틀 화면으로 로딩
        SceneManager.LoadScene(Define.TITLE_SCENE_NAME);
    }

}
