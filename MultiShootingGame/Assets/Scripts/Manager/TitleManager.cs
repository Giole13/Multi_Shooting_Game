using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



// 버튼관련 기능들을 담당하는 클래스
public class TitleManager : MonoBehaviour
{
    [SerializeField] private PhotonManager photonManager;

    [SerializeField] private Transform matchMakingImage;

    [SerializeField] private Transform titleTransform;
    [SerializeField] private Transform settingTransform;


    [SerializeField] private Transform[] selectCharacter;
    [SerializeField] private Transform[] readyImageTransforms;
    [SerializeField] private TMP_Text infoTxt;

    // 캐릭터 선택시 고른 것을 나타내주는 배열
    [SerializeField] private Transform[] playerSelectTransforms;
    [SerializeField] private Button readyPlayBtn;





    private int playerReadyCount = 0;


    // 캐릭터 선택화면으로 바꿔주는 함수
    public void SwitchSelectCharacterScreen()
    {
        // 타이틀 매뉴들 꺼주기
        titleTransform.gameObject.SetActive(false);

        // 캐릭터 선택 화면 보여주기
        foreach (var obj in selectCharacter)
        {
            obj.gameObject.SetActive(true);
        }

        // 멀티플레이라면 플레이어 준비 이미지 켜주기
        if (GameManager.Instance.IsMultiPlay)
        {
            readyImageTransforms[0].parent.gameObject.SetActive(true);
        }
        // 싱글플레이 : 플레이어 준비 이미지 끄기
        else
        {
            readyImageTransforms[0].parent.gameObject.SetActive(false);
        }
    }

    // 싱글 플레이 버튼
    public void SingleGameStartBtn()
    {
        // GameManager.Instance.SceneMove(Define.LOADING_SCENE_NAME);
        // 멀티 플레이 세팅 false
        GameManager.Instance.SettingMultiPlay(false);

        SwitchSelectCharacterScreen();
    }

    // 멀티 플레이 버튼
    public void MultiGameStartBtn()
    {
        // 멀티플레이를 설정하는 함수
        // 멀티 플레이 세팅 true
        photonManager.SettingMultiPlayer();
        GameManager.Instance.SettingMultiPlay(true);

        // 매치메이킹 이미지 켜주기
        matchMakingImage.gameObject.SetActive(true);
    }


    // 멀티 : 게임 준비 완료 버튼 (멀티용 2차 준비)
    public bool ReadyToGamePlay()
    {
        // 플레이어준비가 완료되면 초록색으로 변경해준다.
        Image readyImage;
        readyImageTransforms[playerReadyCount].TryGetComponent<Image>(out readyImage);
        readyImage.color = Color.green;
        playerReadyCount++;


        // 플레이어 준비 수가 초과하면 true 반환
        if (readyImageTransforms.Length <= playerReadyCount)
        {
            return true;
        }

        // 모두 준비가 안되면 false 반환
        return false;
    }

    public void StartCountDownToGameStart(int num)
    {
        infoTxt.text = $"{num}초 후 게임이 시작됩니다.";
    }

    // 게임 플레이 준비 완료 버튼 (1차 준비)
    public void ReadyGamePlayBtn()
    {
        readyPlayBtn.interactable = false;

        // 싱글플레이일 경우 실행하는 로직
        if (GameManager.Instance.IsMultiPlay == false)
        {
            SceneManager.LoadSceneAsync(Define.INGAME_SCENE_NAME);
            return;
        }

        // 멀티플레이일 경우
        photonManager.StartInGame();
    }

    // 설정 버튼
    public void SettingBtn()
    {
        // 타이틀을 꺼주고 세팅 창을 켜준다.
        titleTransform.gameObject.SetActive(false);
        settingTransform.gameObject.SetActive(true);
    }

    // 세팅화면에서 타이틀로 돌아가는 버튼
    public void SettingToBackBtn()
    {
        titleTransform.gameObject.SetActive(true);
        settingTransform.gameObject.SetActive(false);
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

    // 기사 선택 
    public void SelectKnight()
    {
        playerSelectTransforms[0].gameObject.SetActive(true);
        playerSelectTransforms[1].gameObject.SetActive(false);
        GameManager.Instance.CharacterType = PlayerCharacterType.KNIGHT;
    }

    // 총잡이 선택 
    public void SelectGunner()
    {
        playerSelectTransforms[0].gameObject.SetActive(false);
        playerSelectTransforms[1].gameObject.SetActive(true);
        GameManager.Instance.CharacterType = PlayerCharacterType.GUNNER;

    }
}
