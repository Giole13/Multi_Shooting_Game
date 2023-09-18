using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public PlayerBullet()
    {
        bulletName = GetType().ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적과 충돌하면 적 공격받음 실행
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamageable>().BeAttacked(damage);
        }
        gameObject.SetActive(false);
    }
}
