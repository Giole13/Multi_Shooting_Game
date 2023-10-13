using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임에서 사용되는 상수들을 모아놓은 클래스
public class Stats
{
    private Stats() { }
    public Stats(int health, int damage, float speed)
    {
        Health = health;
        Damage = damage;
        Speed = speed;
    }

    // 체력
    public int Health;
    // 공격력
    public int Damage;
    // 이동속도
    public float Speed;
}

// 총의 기본 스펙
public struct GunSpecifications
{
    public GunSpecifications(int gunDamage, float bulletSpeed, float firedDelayTime, int maxAmmoCount, bool isUnlimitedBullets = false)
    {
        GunDamage = gunDamage;
        BulletSpeed = bulletSpeed;
        FiredDelayTime = firedDelayTime;
        MaxAmmoCount = maxAmmoCount;
        IsUnlimitedBullets = isUnlimitedBullets;
        CurrentAmmoCount = MaxAmmoCount;
    }

    public int GunDamage { get; private set; }
    public float BulletSpeed { get; private set; }
    public float FiredDelayTime { get; private set; }

    // 총의 최대 탄약수
    public int MaxAmmoCount { get; private set; }
    // 총의 현재 탄약수
    public int CurrentAmmoCount { get; private set; }

    public bool IsUnlimitedBullets { get; private set; }

    // 총의 현재 탄약수를 줄여주는 함수
    public void DecreaseCurrentAmmo()
    {
        if (IsUnlimitedBullets)
        {
            return;
        }
        CurrentAmmoCount--;
    }

    // 총알을 모두 채우는 함수
    public void IncreaseCurrentAmmo()
    {
        CurrentAmmoCount = MaxAmmoCount;
    }

    // 총의 데미지를 설정하는 함수
    public void InitGunDamage(int damage)
    {
        GunDamage = damage;
    }

}