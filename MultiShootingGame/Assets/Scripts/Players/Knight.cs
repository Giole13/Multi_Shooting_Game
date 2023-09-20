using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어 기사 - 방패로 탄막을 튕겨냄
public class Knight : Player
{
    [SerializeField] private Transform shieldTransform;

    // 초기화 함수
    protected override void Init()
    {
        stats = new Stats(10, 2, 15f);
    }


}
