using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 버튼관련 기능들을 담당하는 클래스
public class ButtonManager : MonoBehaviour
{
    [SerializeField] private PhotonManager photonManager;

    [SerializeField] private Transform matchMakingImage;
    [SerializeField] private Transform[] selectCharacter;

    [SerializeField] private Transform titleTransform;


    // 캐릭터 선택화면으로 바꿔주는 함수
    public void SwitchSelectCharacterScreen()
    {
        titleTransform.gameObject.SetActive(false);

        foreach (var obj in selectCharacter)
        {
            obj.gameObject.SetActive(true);
        }
    }

    // 싱글 플레이 버튼
    public void SingleGameStartBtn()
    {
        // GameManager.Instance.SceneMove(Define.LOADING_SCENE_NAME);

        SwitchSelectCharacterScreen();
    }

    // 멀티 플레이 버튼
    public void MultiGameStartBtn()
    {
        // 멀티플레이를 설정하는 함수
        photonManager.SettingMultiPlayer();
        GameManager.Instance.MultiPlaySetting();

        // 매치메이킹 이미지 켜주기
        matchMakingImage.gameObject.SetActive(true);

        // GameManager.Instance.SceneMove(Define.LOADING_SCENE_NAME);
    }

    // 캐릭터 선택화면에서 게임을 시작하는 함수
    public void InGameStartBtn()
    {
        LoadingManager.Instance.SceneChangeToInGame();
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
