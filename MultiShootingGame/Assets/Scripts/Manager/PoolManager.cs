using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

// 여러 오브젝트 풀링들을 책임지는 클래스
public class PoolManager : MonoBehaviour
{
    // 풀링 객체
    // 딕셔너리에서 바로 접근 후 뽑아 사용하는 형태
    private static Dictionary<string, Stack<GameObject>> objectPool = null;

    private static PoolManager _instance = null;

    public static PoolManager Instance
    {
        get
        {
            // 널인 경우 새로운 객체 생성 및 반환
            if (_instance is null)
            {
                _instance = FindObjectOfType<PoolManager>();
                if (_instance is null)
                {
                    // 자신의 스크립트 이름으로 객체 생성
                    GameObject obj = new GameObject(typeof(PoolManager).ToString());
                    // 해당 객체에 자신 스크립트 추가
                    _instance = obj.AddComponent<PoolManager>();
                }
            }
            return _instance;
        }
    }

    // 풀링할 프리팹 리스트
    [SerializeField] private List<poolObjectInfo> poolList;

    [Serializable]
    private struct poolObjectInfo
    {
        public GameObject obj;
        public int poolSize;
        [ReadOnly]
        public string ObjectName;

        // 멀티플레이에서 동기화할 오브젝트인지 판별하는 변수
        public bool IsMultiPlaySync;
    }

    private void Start()
    {
        // 싱글플레이 기준
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
                GameObject bullet;
                // 멀티 : 동기화를 해야 하는 객체일 경우 PhotonNetwork를 통해 만든다. -> 마스터 기준
                if (GameManager.Instance.IsMultiPlay && poolList[i].IsMultiPlaySync && PhotonNetwork.IsMasterClient)
                {
                    // bullet = PhotonNetwork.PrefabPool.Instantiate(poolList[i].obj.GetComponent<PhotonView>().GetInstanceID().ToString(),
                    //                                                 Vector3.zero, Quaternion.identity);

                    bullet = PhotonNetwork.Instantiate(poolList[i].ObjectName, Vector3.zero, Quaternion.identity);
                    bullet.transform.SetParent(transform.GetChild(i));
                    bullet.name += "_" + j;
                    bullet.SetActive(false);
                    bulletStack.Push(bullet);
                    continue;
                }
                else
                {
                    // 멀티 : 동기화 해야 하는 객체라면(photonView 컴포넌트) 게스트일 경우 넘어가기
                    if (GameManager.Instance.IsMultiPlay && poolList[i].IsMultiPlaySync)
                    {
                        break;
                    }
                    // // 싱글 : 멀티가 아니고 멀티에서 동기화 해야 하는 오브젝트라면 photonView를 제거해준다.
                    // else if ((GameManager.Instance.IsMultiPlay == false) && poolList[i].IsMultiPlaySync)
                    // {
                    //     Destroy(poolList[i].obj.GetComponent<PhotonView>());
                    // }
                    bullet = Instantiate(poolList[i].obj, transform.GetChild(i));
                    bullet.name += "_" + j;
                    bullet.SetActive(false);
                    bulletStack.Push(bullet);
                }
            }

            // 딕셔너리에 추가
            objectPool.Add(poolList[i].ObjectName, bulletStack);
        }
    }

    // 풀의 이름에서 오브젝트를 뽑는 함수
    public GameObject PullItObject(string poolName)
    {
        // 남은 오브젝트가 1이하가 된다면 추가적으로 오브젝트 생성
        if (objectPool[poolName].Count <= 1)
        {
            int addSize = 0;
            int index = 0;
            for (int i = 0; i < poolList.Count; i++)
            {
                // 같은 오브젝트 리스트 찾아서 그 수만큼 늘리기
                if (poolList[i].ObjectName == poolName)
                {
                    addSize = poolList[i].poolSize;
                    index = i;
                }
            }


            Transform parentTransform = transform.GetChild(index);
            GameObject targetObj = Instantiate(objectPool[poolName].Pop(), parentTransform);

            for (int i = 0; i <= addSize; i++)
            {
                targetObj = Instantiate(targetObj, parentTransform);
                targetObj.SetActive(false);
                objectPool[poolName].Push(targetObj);
            }
        }
        return objectPool[poolName].Pop();
    }

    // 풀의 이름에 오브젝트 넣는 함수
    public void InsertObject(string poolName, GameObject obj)
    {
        objectPool[poolName].Push(obj);
    }


}