using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총 배이스
public class Gun : MonoBehaviour, IGun
{
    [SerializeField] protected int damage;
    [SerializeField] protected int GunDamage;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float FiredDelayTime;
    [SerializeField] protected PlayerInputHandler playerInputHandler;
    protected WaitForSeconds gunFireDelay;
    protected bool IsFire2;
    protected IBullet bullet;
    protected bool IsFire;





    // 기본 공격 함수, 공격중일 경우 계속 공격
    public void BulletFire()
    {
        StartCoroutine(FireDefualtGunRoutine());
    }

    // 기본 총의 공격 로직
    protected virtual IEnumerator FireDefualtGunRoutine()
    {
        IsFire = true;
        while (IsFire && IsFire2)
        {
            // 풀매니저에서 총알을 참조하고
            PoolManager.Instance.PullItObject("PlayerBullet").TryGetComponent<IBullet>(out bullet);
            // 슈팅만 하면 됨
            bullet.ShottingBullet(transform.right * bulletSpeed, transform.position, damage);
            IsFire2 = false;
            yield return gunFireDelay;
            IsFire2 = true;
        }
    }

    // 무기를 획득하면 무기의 값을 초기화
    public void Init(Player player)
    {
        SettingGun();

        IsFire2 = true;
        damage = player.stats.Damage * GunDamage;
        player.transform.TryGetComponent<PlayerInputHandler>(out playerInputHandler);
        transform.localPosition = new Vector2(1f, 0f);
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        gunFireDelay = new WaitForSeconds(FiredDelayTime);
    }

    // 각 무기마다 초기 세팅값을 설정
    public virtual void SettingGun()
    {
        bulletSpeed = 10f;
        FiredDelayTime = 1f;
        GunDamage = 1;
    }



    // 플레이어와 부딪히면 플레이어의 총을 해당 총으로 바꾼다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IGunChangeable gunChangeable;
            other.TryGetComponent<IGunChangeable>(out gunChangeable);
            gunChangeable.ChangeWeapon(transform);
        }
    }

    // 공격을 멈춘다.
    public void BulletFireStop()
    {
        IsFire = false;
    }
}
