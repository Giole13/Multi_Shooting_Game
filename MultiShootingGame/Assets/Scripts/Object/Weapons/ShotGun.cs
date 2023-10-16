using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 샷건
public class ShotGun : Gun
{
    // 공격시 나갈 총알의 개수
    [ReadOnly] private int totalNumberBullets = 5;

    [SerializeField] private Transform bulletSpawnPoint;

    // 각각의 총알사이의 각도
    private float shootAngle = 10f;

    // 값 초기화
    public override void SettingGun()
    {
        gunSpec = new GunSpecifications(1, 20f, 1f, 50);
    }

    //샷건의 공격 로직
    protected override IEnumerator FireGunRoutine()
    {
        yield return null;

        IsFire = true;
        while (IsFire && IsFire2)
        {
            // 산탄으로 발사하는 로직
            float angle = Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

            bulletSpawnPoint.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            bulletSpawnPoint.Rotate(new Vector3(0, 0, shootAngle * (totalNumberBullets / 2)));

            for (int i = 0; i < totalNumberBullets; i++)
            {
                // 플레이어가 가지고 있는 총이라면
                if (IsPlayerWeapon)
                {
                    if (gunSpec.CurrentAmmoCount <= 0)
                    {
                        yield break;
                    }
                    // 총알을 소모하는 함수
                    gunSpec.DecreaseCurrentAmmo();
                    // 로컬 UI 갱신 
                    UpdateAmmoUI();

                }

                // 풀매니저에서 총알을 참조하고
                PoolManager.Instance.PullItObject(useBulletName).TryGetComponent<IBullet>(out bullet);
                // 슈팅만 하면 됨
                bullet.ShootingBullet(bulletSpawnPoint.up * gunSpec.BulletSpeed, transform.position, transform.parent.rotation, gunSpec.GunDamage);

                bulletSpawnPoint.Rotate(new Vector3(0, 0, -shootAngle));
            }


            IsFire2 = false;
            yield return gunFireDelay;
            IsFire2 = true;
        }
    }

}
