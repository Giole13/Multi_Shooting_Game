using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적과 충돌하면 적 끄기
        if (other.tag == "Enemy")
        {
            other.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
