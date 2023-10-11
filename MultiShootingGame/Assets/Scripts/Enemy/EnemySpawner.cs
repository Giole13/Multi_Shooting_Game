using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private WaitForSeconds createCycleTime = new WaitForSeconds(2f);

    [SerializeField] private List<Transform> spawnPoint;
    [SerializeField] private Transform bossTransform;

    [ReadOnly] private int maxSpawnCount = 1;

    [ReadOnly] private int currentSpawnCount;

    private void Awake()
    {

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
            Instantiate(bossTransform.gameObject);
            bossTransform.gameObject.SetActive(false);
            StartCoroutine(EnemyCreate());
        }
    }


    // 멀티 환경에서 딕셔너리에 추가하는 것을 고려하여 일정 시간 후 스폰
    // 일정 시간마다 랜덤 스폰 포인터에 적 소환
    private IEnumerator EnemyCreate()
    {
        yield return new WaitForSeconds(4f);

        if (PhotonNetwork.IsMasterClient && GameManager.Instance.IsMultiPlay)
        {
            // 마스터 클라이언트에서 처리
            bossTransform = PhotonNetwork.Instantiate("Boss", Vector3.zero, Quaternion.identity).transform;
            bossTransform.gameObject.SetActive(false);
        }


        int randomIndex;
        while (true)
        {
            randomIndex = UnityEngine.Random.Range(0, spawnPoint.Count);
            yield return createCycleTime;
            // 최대 스폰까지 도달한다면 멈추고 보스 소환
            if (currentSpawnCount >= maxSpawnCount)
            {


                bossTransform.GetComponent<ISetPosition>().SetPosition(spawnPoint[randomIndex].position);
                yield break;
            }

            PoolManager.Instance.PullItObject("Enemy").GetComponent<ISetPosition>().SetPosition(spawnPoint[randomIndex].position);
            currentSpawnCount++;
        }
    }

}
