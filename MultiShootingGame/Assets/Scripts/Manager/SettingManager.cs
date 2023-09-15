using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 설정들을 초기화 해주는 클래스
public class SettingManager : MonoBehaviour
{



    // 게임 시작시 기본 세팅 초기화
    void Start()
    {
        // 전체화면 FHD 해상도 설정
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }


}
