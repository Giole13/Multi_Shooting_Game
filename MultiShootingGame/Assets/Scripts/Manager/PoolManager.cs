using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 여러 오브젝트 풀링들을 책임지는 클래스
public class PoolManager : Singleton<PoolManager>
{
    // 풀링 객체
    // 딕셔너리에서 바로 접근 후 뽑아 사용하는 형태
    // [SerializeField] private Dictionary<string, Stack<GameObject>> bulletPool;

    // // 풀링할 프리팹 리스트
    // [SerializeField] private List<GameObject> bulletList;

    [SerializeField] private int poolSize;

    [SerializeField] private GameObject playerBullet;
    [SerializeField] private GameObject enemyBullet;

    private Stack<GameObject> bulletStack;
    private void Start()
    {
        bulletStack = new Stack<GameObject>();

        // poolSize 만큼 오브젝트 생성
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(playerBullet, transform);
            bulletStack.Push(obj);
            obj.name = "Bullet_" + i;
            obj.SetActive(false);
        }

    }

    // 총알을 뽑는 변수
    public GameObject PullItBullet()
    {
        GameObject obj = bulletStack.Pop();
        return obj;
    }

    public void InsertBullet(GameObject obj)
    {
        bulletStack.Push(obj);
    }


}
