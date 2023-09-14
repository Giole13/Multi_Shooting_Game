using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 발사 주기는 1초
    private WaitForSeconds fireCycle = new WaitForSeconds(1f);

    [SerializeField] private Transform playerTransform;

    [SerializeField] private GameObject bulletObj;
    private Vector2 bulletDir;
    private IBullet bullet;

    private int damage;
    void Start()
    {
        damage = 10;
        StartCoroutine(FireBullet());
    }

    private void Update()
    {
        // 총알 위치는 플레이어 방향
        bulletDir = (playerTransform.position - transform.position).normalized;
    }

    private IEnumerator FireBullet()
    {
        while (true)
        {
            yield return fireCycle;
            bullet = Instantiate(bulletObj).GetComponent<IBullet>();
            bullet.ShottingBullet(bulletDir, transform.position, damage);
        }
    }

}
