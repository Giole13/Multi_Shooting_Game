using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 설정들을 초기화 해주는 클래스
public class SettingManager : MonoBehaviour
{
    [SerializeField] private List<Transform> initTransformList;


    // 게임 시작시 기본 세팅 초기화
    void Start()
    {
        // 처음에 꺼두어야 하는 오브젝트를 전부 꺼주기
        foreach (Transform offTransform in initTransformList)
        {
            offTransform.gameObject.SetActive(false);
        }


        // 전체화면 FHD 해상도 설정
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }


}
