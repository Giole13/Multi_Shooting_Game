using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    public EnemyBullet()
    {
        bulletName = GetType().ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌하면
        if (other.tag == "Player")
        {
            // other.gameObject.SetActive(false);
            Debug.Log("충돌!");
        }
        gameObject.SetActive(false);
    }

}
