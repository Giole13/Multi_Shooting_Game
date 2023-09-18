using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 샷건
public class ShotGun : Gun
{
    // 공격시 나갈 총알의 개수
    [ReadOnly] private int totalNumberBullets = 13;

    [SerializeField] private Transform bulletSpawnPoint;
    private float shootAngle = 5f;

    // 값 초기화
    public override void SettingGun()
    {
        bulletSpeed = 20f;
        FiredDelayTime = 0.3f;
        GunDamage = 1;
    }

    //샷건의 공격 로직
    protected override IEnumerator FireDefualtGunRoutine()
    {
        IsFire = true;
        while (IsFire && IsFire2)
        {
            // 산탄으로 발사하는 로직
            Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - bulletSpawnPoint.position).normalized;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            bulletSpawnPoint.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            bulletSpawnPoint.Rotate(new Vector3(0, 0, shootAngle * (totalNumberBullets / 2)));


            for (int i = 0; i < totalNumberBullets; i++)
            {
                // 풀매니저에서 총알을 참조하고
                PoolManager.Instance.PullItObject("PlayerBullet").TryGetComponent<IBullet>(out bullet);
                // 슈팅만 하면 됨
                bullet.ShottingBullet(bulletSpawnPoint.up * bulletSpeed, transform.position, damage);

                bulletSpawnPoint.Rotate(new Vector3(0, 0, -shootAngle));
            }



            IsFire2 = false;
            yield return gunFireDelay;
            IsFire2 = true;
        }
    }

}
