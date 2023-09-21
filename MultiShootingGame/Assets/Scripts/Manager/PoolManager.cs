using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 여러 오브젝트 풀링들을 책임지는 클래스
public class PoolManager : Singleton<PoolManager>
{
    // 풀링 객체
    // 딕셔너리에서 바로 접근 후 뽑아 사용하는 형태
    private static Dictionary<string, Stack<GameObject>> objectPool = null;


    // 풀링할 프리팹 리스트
    [SerializeField] private List<poolObjectInfo> poolList;

    [Serializable]
    private struct poolObjectInfo
    {
        public GameObject obj;
        public int poolSize;
        [ReadOnly]
        public string ObjectName;
    }

    private void Awake()
    {
        objectPool = new Dictionary<string, Stack<GameObject>>();
        // 각 오브젝트들의 이름들을 초기화한다.
        for (int i = 0; i < poolList.Count; i++)
        {
            // 이름 초기화
            poolObjectInfo temp = poolList[i];
            temp.ObjectName = poolList[i].obj.name;
            poolList[i] = temp;

            GameObject bundle = new GameObject(poolList[i].ObjectName + "bundle");
            Instantiate(bundle, transform);
            Destroy(bundle);

            Stack<GameObject> bulletStack = new Stack<GameObject>();
            // 풀 사이즈만큼 풀링 초기화
            for (int j = 0; j < poolList[i].poolSize; j++)
            {
                GameObject bullet = Instantiate(poolList[i].obj, transform.GetChild(i));
                bullet.name += "_" + j;
                bullet.SetActive(false);
                bulletStack.Push(bullet);
            }

            // 딕셔너리에 추가
            objectPool.Add(poolList[i].ObjectName, bulletStack);
        }
    }

    // 풀의 이름에서 오브젝트를 뽑는 함수
    public GameObject PullItObject(string poolName)
    {
        return objectPool[poolName].Pop();
    }

    // 풀의 이름에 오브젝트 넣는 함수
    public void InsertObject(string poolName, GameObject obj)
    {
        objectPool[poolName].Push(obj);
    }
}