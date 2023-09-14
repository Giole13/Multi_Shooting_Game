using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여러 오브젝트 풀링들을 책임지는 클래스
public class PoolManager : MonoBehaviour
{
    // 풀링 객체
    // 딕셔너리에서 바로 접근 후 뽑아 사용하는 형태
    [SerializeField] private Dictionary<string, Stack<GameObject>> bulletPool;

    // 풀링할 프리팹 리스트
    [SerializeField] private List<GameObject> bulletList;

    private void Awake()
    {

    }


}
