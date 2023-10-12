using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 설정들을 초기화 해주는 클래스
public class SettingManager : MonoBehaviour
{
    // 게임 시작시 비활성화 해야 하는 것들
    [SerializeField] private List<Transform> initTransformListToAtiveFalse;

    // 게임 시작시 활성화 해야 하는 것들
    [SerializeField] private List<Transform> initTransformListToAtiveTrue;

    private bool isFullScreen;


    // 게임 시작시 기본 세팅 초기화
    void Start()
    {
        // 처음에 꺼두어야 하는 오브젝트를 전부 꺼주기
        foreach (Transform offTransform in initTransformListToAtiveFalse)
        {
            offTransform.gameObject.SetActive(false);
        }

        // 처음에 활성화 해야하는 오브젝트 전부 켜주기
        foreach (Transform onTransform in initTransformListToAtiveTrue)
        {
            onTransform.gameObject.SetActive(true);
        }


        // 전체화면 FHD 해상도 설정
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        isFullScreen = false;
    }

    public void SetFullScreen()
    {
        // 전체화면일 때 -> 창모드로 변경
        if (isFullScreen)
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            isFullScreen = false;

        }
        // 창모드 일때 -> 전체화면으로 변경
        else
        {
            Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
            isFullScreen = true;
        }
    }
}
