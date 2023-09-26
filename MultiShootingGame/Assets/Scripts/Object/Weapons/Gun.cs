using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


// 총 배이스
public class Gun : MonoBehaviour, IGun
{
    protected GunSpecifications gunSpec;

    protected WaitForSeconds gunFireDelay;
    protected IBullet bullet;

    protected bool IsFire2;
    protected bool IsFire;

    protected string useBulletName;
    protected bool IsPlayerWeapon;


    // 멀티 : 로컬인지 확인하는 함수
    protected bool IsLocalItem;


    private void Awake()
    {
        SettingGun();
    }

    // 각 무기마다 초기 세팅값을 설정
    public virtual void SettingGun()
    {
        gunSpec = new GunSpecifications(1, 10f, 1f, 10, true);
    }

    // 기본 공격 함수, 공격중일 경우 계속 공격
    public void BulletFire()
    {
        if (gunSpec.CurrentAmmoCount <= 0)
        {
            return;
        }
        StartCoroutine(FireGunRoutine());
    }

    // 상위 객체로 들어온 후 실행
    // 무기를 획득하면 무기의 값을 초기화, 사용할 총알의 이름 초기화
    public void Init()
    {
        switch (transform.parent.tag)
        {
            // 플레이어라면 플레이어블 무기로 설정, 탄약개수 UI 초기화
            case "Player":
                useBulletName = Define.PLAYER_BULLET_NAME;
                IsPlayerWeapon = true;
                gunSpec.InitGunDamage(GameManager.Instance.Player.stats.Damage);
                break;
            case "Enemy":
                useBulletName = Define.ENEMY_BULLET_NAME;
                IsPlayerWeapon = false;
                break;
            default:
                Debug.Assert(false, "Gun : 설정된 태그와 맞는 케이스가 없습니다.");
                break;
        }

        GetComponent<Collider2D>().enabled = false;
        IsFire2 = true;
        transform.localPosition = new Vector2(1f, 0f);
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        gunFireDelay = new WaitForSeconds(gunSpec.FiredDelayTime);

        // 싱글, 멀티에서 자기 자신은 로컬로 판정
        IsLocalItem = true;
    }

    // 로컬 객체가 아니게 해주는 함수
    public void MakeItNonLocal()
    {
        IsLocalItem = false;
    }

    // 기본 총의 공격 로직
    protected virtual IEnumerator FireGunRoutine()
    {
        yield return null;
        IsFire = true;
        while (IsFire && IsFire2)
        {
            // 풀매니저에서 총알을 참조하고
            PoolManager.Instance.PullItObject(useBulletName).TryGetComponent<IBullet>(out bullet);
            // 슈팅만 하면 됨
            bullet.ShottingBullet(transform.right * gunSpec.BulletSpeed, transform.position, gunSpec.GunDamage);

            // 플레이어가 가지고 있는 총이라면
            if (IsPlayerWeapon)
            {
                if (gunSpec.CurrentAmmoCount <= 0)
                {
                    yield break;
                }

                // 총알을 소모하는 함수
                gunSpec.CurrentAmmoCountDown();

                RenewalAmmoUI();
            }

            IsFire2 = false;
            yield return gunFireDelay;
            IsFire2 = true;
        }
    }


    // 플레이어와 부딪히면 플레이어의 총을 해당 총으로 바꾼다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IGunFirstAcquisition>().FirstAcquisitionChangeWeapon(transform);
        }
    }

    // 공격을 멈춘다.
    public void BulletFireStop()
    {
        IsFire = false;
    }

    // 총알 UI 갱신하는 함수
    public void RenewalAmmoUI()
    {
        // 로컬에서만 실행해야 하는 함수
        if (IsLocalItem)
        {
            // 총알 UI 를 갱신하는 함수
            GameManager.Instance.PlayerStatsUI.
                SetAmmoTxet(gunSpec.CurrentAmmoCount, gunSpec.MaxAmmoCount, gunSpec.IsUnlimitedBullets);
        }
    }
}
