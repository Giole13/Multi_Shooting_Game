using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private WaitForSeconds createCycleTime = new WaitForSeconds(2f);

    [SerializeField] private List<Transform> spawnPoint;
    [SerializeField] private Transform bossTransform;

    [SerializeField] private int maxSpawnCount = 5;
    [ReadOnly] private int currentSpawnCount;

    public EnemySpawner()
    {
        currentSpawnCount = 0;
    }

    private void Awake()
    {
        // 몬스터 스폰 켜주기
        StartCoroutine(EnemyCreate());
        // 보스는 꺼두기
        bossTransform.gameObject.SetActive(false);
    }


    // 일정 시간마다 랜덤 스폰 포인터에 적 소환
    private IEnumerator EnemyCreate()
    {
        int randomIndex;
        while (true)
        {
            randomIndex = UnityEngine.Random.Range(0, spawnPoint.Count);
            // 최대 스폰까지 도달한다면 멈추고 보스 소환
            if (currentSpawnCount >= maxSpawnCount)
            {
                bossTransform.GetComponent<ISetPosition>().SetPosition(spawnPoint[randomIndex].position);
                bossTransform.gameObject.SetActive(true);
                yield break;
            }
            yield return createCycleTime;
            PoolManager.Instance.PullItObject("Enemy").GetComponent<ISetPosition>().SetPosition(spawnPoint[randomIndex].position);
            currentSpawnCount++;
        }

    }

}
