using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ISetPosition
{
    // 발사 주기는 1초
    private WaitForSeconds fireCycle = new WaitForSeconds(1f);

    private Transform playerTransform;
    private Rigidbody2D enemyRigid;
    private Vector2 bulletDir;
    private IBullet bullet;
    private int damage;

    private bool life = false;

    void Start()
    {
        damage = 10;
        StartCoroutine(FireBullet());
        enemyRigid = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        // 움직임 계산
        Vector2 nextVec = bulletDir * Time.fixedDeltaTime;
        enemyRigid.MovePosition(enemyRigid.position + nextVec);
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
            bullet = PoolManager.Instance.PullItObject("EnemyBullet").GetComponent<IBullet>();
            bullet.ShottingBullet(bulletDir, transform.position, damage);
        }
    }

    // 몬스터가 소환되는 함수
    public void SetPosition(Vector2 pos)
    {
        life = true;
        transform.position = pos;
        gameObject.SetActive(true);
        playerTransform = GameManager.Instance.PlayerTransform;
    }

    private void OnDisable()
    {
        if (life)
        {
            life = false;
            PoolManager.Instance.InsertObject("Enemy", gameObject);
        }
    }
}
