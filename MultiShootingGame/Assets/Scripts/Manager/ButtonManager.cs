using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 버튼관련 기능들을 담당하는 클래스
public class ButtonManager : MonoBehaviour
{

    // 싱글 플레이 버튼
    public void SingleGameStartBtn()
    {
        if (PoolManager.Instance.gameObject is null)
        {

        }
        PoolManager.Instance.gameObject.SetActive(true);
        GameManager.Instance.SceneMove(Define.INGAME_SCENE_NAME);

    }

    // 멀티 플레이 버튼
    public void MultiGameStartBtn()
    {
        // 멀티플레이를 설정하는 함수
        // 2023.09.14 / HyungJun / 싱글플레이 작업 완료 후 작업
        // GameManager.instance.MultiPlaySetting();
        // GameManager.instance.SceneMove(SceneNameDefine.LOADING_SCENE_NAME);

    }

    // 설정 버튼
    public void SettingBtn()
    {

    }

    // 데스크톱으로 나가기 버튼
    public void QuitGameBtn()
    {
        GameManager.Instance.ExitDesktop();
    }

    // 타이틀 이동 버튼
    public void BackToTitle()
    {
        GameManager.Instance.SceneMove(Define.TITLE_SCENE_NAME);

    }
}
