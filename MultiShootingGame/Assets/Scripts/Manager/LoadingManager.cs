using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 캐릭터 선택창과 인게임을 비동기로 불러오는 클래스
public class LoadingManager : Singleton<LoadingManager>
{
    [ReadOnly] private AsyncOperation selectCharacterSceneOperation;
    [ReadOnly] private AsyncOperation inGameSceneOperation;


    private bool IsCharacterSceneLoaded;
    private bool IsIngameLoaded;

    // 엔딩에서 전부 초기화 해주는 함수
    public override void Init()
    {
        IsCharacterSceneLoaded = false;
        IsIngameLoaded = false;


        selectCharacterSceneOperation = null;
        inGameSceneOperation = null;
    }


    // 로딩을 시작하는 함수
    public void StartLoadingSequence()
    {
        Init();

        StartCoroutine(LoadingCharacterScene());
        StartCoroutine(LoadingIngameScene());

        DontDestroyOnLoad(gameObject);
    }


    // 캐릭터 씬을 불러우는 코루틴
    private IEnumerator LoadingCharacterScene()
    {
        // 캐릭터 선택 씬을 불러올 준비
        selectCharacterSceneOperation = SceneManager.LoadSceneAsync(Define.SELECTCHARACTER_SCENE_NAME);
        selectCharacterSceneOperation.allowSceneActivation = false;
        selectCharacterSceneOperation.priority = 1;

        yield return new WaitUntil(() => 0.89f <= selectCharacterSceneOperation.progress);
        Debug.Log($"캐릭터 선택씬 로딩 진행도 : {selectCharacterSceneOperation.progress}");

        IsCharacterSceneLoaded = true;

        // 두 씬 모두 준비되면 캐릭터 씬으로 넘기기
        yield return new WaitUntil(() => IsCharacterSceneLoaded && IsIngameLoaded);
        selectCharacterSceneOperation.allowSceneActivation = true;


        // while (true)
        // {
        //     if (0.89f <= selectCharacterSceneOperation.progress)
        //     {
        //         // selectCharacterSceneOperation.priority = 2;
        //         break;
        //     }
        //     yield return null;
        // }
    }

    // 인게임 씬을 불러우는 코루틴
    private IEnumerator LoadingIngameScene()
    {
        // 인게임 씬을 불러올 준비
        inGameSceneOperation = SceneManager.LoadSceneAsync(Define.INGAME_SCENE_NAME);
        inGameSceneOperation.allowSceneActivation = false;
        inGameSceneOperation.priority = 2;

        yield return new WaitUntil(() => 0.89f <= inGameSceneOperation.progress);
        Debug.Log($"인게임 로딩 진행도 : {inGameSceneOperation.progress}");

        IsIngameLoaded = true;

        // 캐릭터 씬으로 씬 전환
        // selectCharacterSceneOperation.allowSceneActivation = true;

        // while (true)
        // {
        //     if (0.89f <= inGameSceneOperation.progress)
        //     {
        //         // 인게임까지 불러온다면 씬 전환
        //         selectCharacterSceneOperation.allowSceneActivation = true;
        //         yield break;
        //     }
        //     yield return null;
        // }
    }

    // 인게임 씬으로 넘겨주는 함수
    public void SceneChangeToInGame()
    {
        SceneManager.LoadSceneAsync(Define.INGAME_SCENE_NAME);
        // Debug.Log("클릭!");
        // // selectCharacterSceneOperation.priority = 1;
        // // inGameSceneOperation.priority = 2;
        // inGameSceneOperation.allowSceneActivation = true;
    }
}
