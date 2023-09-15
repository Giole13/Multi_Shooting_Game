using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private WaitForSeconds createCycleTime = new WaitForSeconds(2f);

    [SerializeField] private List<Transform> spawnPoint;



    private void Awake()
    {
        StartCoroutine(EnemyCreate());
    }

    // 일정 시간마다 랜덤 스폰 포인터에 적 소환
    private IEnumerator EnemyCreate()
    {
        int randomIndex;
        while (true)
        {
            yield return createCycleTime;
            randomIndex = Random.Range(0, spawnPoint.Count);
            PoolManager.Instance.PullItObject("Enemy").GetComponent<ISetPosition>().SetPosition(spawnPoint[randomIndex].position);
        }
    }

}
