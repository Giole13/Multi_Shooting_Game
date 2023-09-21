using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 매번 씬의 로드마다 로딩을 시작하는 클래스
public class LoadingInit : MonoBehaviour
{
    void Awake()
    {
        // 씬을 비동기로 로드한다.
        LoadingManager.Instance.StartLoadingSequence();
    }

}
