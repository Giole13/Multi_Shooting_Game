using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 발사 주기는 1초
    private WaitForSeconds fireCycle = new WaitForSeconds(1f);

    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject bulletObj;

    private Rigidbody2D enemyRigid;
    private Vector2 bulletDir;
    private IBullet bullet;

    private int damage;
    void Start()
    {
        damage = 10;
        StartCoroutine(FireBullet());
        enemyRigid = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        // 적이 사라지면 엔딩으로 넘어가기
        GameManager.Instance.SceneMove(Define.ENDING_SCENE_NAME);
    }

    private void FixedUpdate()
    {
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
            bullet = Instantiate(bulletObj).GetComponent<IBullet>();
            bullet.ShottingBullet(bulletDir, transform.position, damage);
        }
    }

}
