using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    private Rigidbody2D bulletRigid;
    // 총알의 데미지
    private int damage;

    private void OnEnable()
    {
        StartCoroutine(disappearBullet());
    }

    // 5초 후 꺼주기
    private IEnumerator disappearBullet()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    // 총알을 발사하는 것을 구현
    public void ShottingBullet(Vector2 dir, Vector2 pos, int _damage)
    {
        bulletRigid = GetComponent<Rigidbody2D>();
        transform.position = pos;
        bulletRigid.velocity = dir * 10f;
        damage = _damage;
    }
}
