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