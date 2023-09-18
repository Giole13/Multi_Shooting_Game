using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스를 책임지는 클래스
public class Boss : Enemy
{
    // 보스의 스탯을 초기화
    protected override void Init()
    {
        stats = new Stats(20, 2, 20f);
    }

    // protected virtual IEnumerator FireBullet()
    // {
    //     while (true)
    //     {
    //         yield return fireCycle;
    //         bullet = PoolManager.Instance.PullItObject("EnemyBullet").GetComponent<IBullet>();
    //         bullet.ShottingBullet(bulletDir, transform.position, stats.Damage);
    //     }
    // }
}
