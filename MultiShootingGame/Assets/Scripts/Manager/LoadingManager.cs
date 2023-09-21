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


    // 엔딩에서 전부 초기화 해주는 함수
    public override void Init()
    {
        selectCharacterSceneOperation = null;
        inGameSceneOperation = null;
    }


    // 로딩을 시작하는 함수
    public void StartLoadingSequence()
    {
        StartCoroutine(LoadingCoroutine());
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator LoadingCoroutine()
    {
        // 캐릭터 선택 씬을 불러올 준비
        selectCharacterSceneOperation = SceneManager.LoadSceneAsync(Define.SELECTCHARACTER_SCENE_NAME);
        selectCharacterSceneOperation.priority = 1;
        selectCharacterSceneOperation.allowSceneActivation = false;

        while (true)
        {
            if (0.89f <= selectCharacterSceneOperation.progress)
            {
                selectCharacterSceneOperation.priority = 2;
                break;
            }
            yield return null;
        }

        // 인게임 씬을 불러올 준비
        inGameSceneOperation = SceneManager.LoadSceneAsync(Define.INGAME_SCENE_NAME);
        inGameSceneOperation.priority = 1;
        inGameSceneOperation.allowSceneActivation = false;

        while (true)
        {
            if (0.89f <= inGameSceneOperation.progress)
            {
                // 인게임까지 불러온다면 씬 전환
                selectCharacterSceneOperation.allowSceneActivation = true;
                yield break;
            }
            yield return null;
        }

        while (true)
        {
            // 두 씬 거의 다 불러오면 씬을 전환한다.
            if (0.89f <= selectCharacterSceneOperation.progress && 0.89f <= inGameSceneOperation.progress)
            {
                Debug.Log("모든 씬 불러왔음 !");
                // 캐릭터 선택 씬으로 넘기고 코루틴 종료
                selectCharacterSceneOperation.allowSceneActivation = true;
                yield break;
            }

            // Debug.Log($"씬 불러오는중 !!");
            // Debug.Log($"캐릭터 선택 씬 진행도! : {selectCharacterSceneOperation.progress}");
            // Debug.Log($"캐릭터 선택 씬 완료! : {selectCharacterSceneOperation.isDone}");
            // Debug.Log($"캐릭터 선택 씬 우선순위! : {selectCharacterSceneOperation.priority}");
            // Debug.Log($"인게임 씬 진행도! : {inGameSceneOperation.progress}");
            // Debug.Log($"인게임 씬 완료! : {inGameSceneOperation.isDone}");
            // Debug.Log($"인게임 씬 우선순위! : {inGameSceneOperation.priority}");
            yield return null;
        }
    }

    // 인게임 씬으로 넘겨주는 함수
    public void SceneChangeToInGame()
    {
        inGameSceneOperation.allowSceneActivation = true;
    }
}
