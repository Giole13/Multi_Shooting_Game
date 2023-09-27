using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 보스를 책임지는 클래스
public class Boss : Enemy
{
    // 보스의 스탯을 초기화
    protected override void Init()
    {
        stats = new Stats(20, 2, 2f);
        // 풀링을 사용하지 않는다.
        IsPoolInsert = false;
    }

    // 공격을 받는 함수
    public override void BeAttacked(int damage)
    {
        stats.Health -= damage;
        // 체력이 0 이하가 됬을 때 몬스터는 사라진다.
        if (stats.Health <= 0)
        {
            // 보스가 죽었을 때는 엔딩이 나온다.
            gameObject.SetActive(false);
            SceneManager.LoadSceneAsync(Define.ENDING_SCENE_NAME);
            return;
        }
        bool IsKnockback = false;
        // 넉백하는 코루틴
        IEnumerator Knockback()
        {
            // 넉백중이면 코루틴 취소
            if (IsKnockback)
            {
                yield break;
            }
            stats.Speed *= -1;
            IsKnockback = true;
            yield return KnockbackTime;
            IsKnockback = false;
            stats.Speed *= -1;
        }
        StartCoroutine(Knockback());
    }
}
