using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

// 플레이어의 기본정보 클래스 
public class Player : MonoBehaviour, IDamageable
{
    public Stats stats;

    private void Awake()
    {
        stats = new Stats(5, 1, 10f);
    }

    public virtual void BeAttacked(int damage)
    {
        stats.Health -= damage;
        if (stats.Health <= 0)
        {
            Debug.Log("플레이어 사망!");
        }
    }


}
