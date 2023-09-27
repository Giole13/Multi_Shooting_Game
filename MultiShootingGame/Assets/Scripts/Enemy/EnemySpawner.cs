using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EnemySpawner : MonoBehaviourPun
{
    private WaitForSeconds createCycleTime = new WaitForSeconds(2f);

    [SerializeField] private List<Transform> spawnPoint;
    [SerializeField] private Transform bossTransform;

    [ReadOnly] private int maxSpawnCount = 1;

    [ReadOnly] private int currentSpawnCount;

    private void Awake()
    {
        bossTransform.gameObject.SetActive(false);

        // 멀티 : 마스터 클라이언트에서 처리해주기
        if (PhotonNetwork.IsMasterClient && GameManager.Instance.IsMultiPlay)
        {
            // 몬스터 스폰 켜주기
            StartCoroutine(EnemyCreate());
            // 보스는 꺼두기
        }

        // 싱글 : 몬스터 스포너 작동
        else if (GameManager.Instance.IsMultiPlay == false)
        {
            StartCoroutine(EnemyCreate());
        }
    }


    // 일정 시간마다 랜덤 스폰 포인터에 적 소환
    private IEnumerator EnemyCreate()
    {
        int randomIndex;
        while (true)
        {
            randomIndex = UnityEngine.Random.Range(0, spawnPoint.Count);
            yield return createCycleTime;
            // 최대 스폰까지 도달한다면 멈추고 보스 소환
            if (currentSpawnCount >= maxSpawnCount)
            {
                bossTransform.GetComponent<ISetPosition>().SetPosition(spawnPoint[randomIndex].position);
                bossTransform.gameObject.SetActive(true);
                yield break;
            }

            PoolManager.Instance.PullItObject("Enemy").GetComponent<ISetPosition>().SetPosition(spawnPoint[randomIndex].position);
            currentSpawnCount++;
        }
    }

}
