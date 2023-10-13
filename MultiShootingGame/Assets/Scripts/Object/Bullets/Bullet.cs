using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    protected string bulletName;
    private Rigidbody2D bulletRigid;
    // 총알의 데미지
    protected int damage;
    private bool life = false;

    public void OnDisable()
    {
        if (life)
        {
            PoolManager.Instance.InsertObject(bulletName, gameObject);
            life = false;
        }
    }

    // 총알을 발사하는 것을 구현
    public void ShootingBullet(Vector2 dir, Vector2 pos, Quaternion rot, int _damage)
    {
        gameObject.SetActive(true);
        life = true;
        bulletRigid = GetComponent<Rigidbody2D>();
        transform.position = pos;
        transform.rotation = rot;
        bulletRigid.velocity = dir;
        damage = _damage;
        StartCoroutine(FadeAwayTimeBullet());
    }

    private IEnumerator FadeAwayTimeBullet()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
