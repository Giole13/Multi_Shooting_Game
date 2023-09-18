using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총 배이스
public class Gun : MonoBehaviour, IGun
{
    #region 자식클래스에서 커스텀 가능
    protected int damage;
    protected float bulletSpeed;
    protected float FiredDelayTime;
    protected PlayerInputHandler playerInputHandler;
    #endregion 자식클래스에서 커스텀 가능


    #region 자식클래스에서 접근 불가능
    private IBullet bullet;
    private bool IsFire;
    private bool IsFire2;

    private WaitForSeconds gunFireDelay;
    #endregion 자식클래스에서 접근 불가능


    // 기본 공격 함수, 공격중일 경우 계속 공격
    public virtual void BulletFire()
    {
        StartCoroutine(FireDefualtGunRoutine());
    }

    // 기본 총의 공격 로직
    private IEnumerator FireDefualtGunRoutine()
    {
        IsFire = true;
        while (IsFire && IsFire2)
        {
            // 풀매니저에서 총알을 참조하고
            PoolManager.Instance.PullItObject("PlayerBullet").TryGetComponent<IBullet>(out bullet);
            // 슈팅만 하면 됨
            bullet.ShottingBullet(playerInputHandler.bulletDir
                                    * bulletSpeed, transform.position, damage);
            IsFire2 = false;
            yield return gunFireDelay;
            IsFire2 = true;
        }
        Debug.Log("공격끝!");
    }

    // 무기를 획득하면 무기의 값을 초기화
    public virtual void Init(Player player)
    {
        bulletSpeed = 1f;
        FiredDelayTime = 1f;
        IsFire2 = true;

        player.transform.TryGetComponent<PlayerInputHandler>(out playerInputHandler);
        damage = player.stats.Damage;
        transform.localPosition = new Vector2(1f, 0f);
        gunFireDelay = new WaitForSeconds(FiredDelayTime);
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
