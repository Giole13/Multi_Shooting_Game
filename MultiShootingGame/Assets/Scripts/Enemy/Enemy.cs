using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, ISetPosition, IDamageable
{
    [SerializeField] protected Gun enemyGun;


    [SerializeField] protected float bulletSpeed = 10;

    protected Stats stats;
    protected IBullet bullet;
    protected Vector2 bulletDir;

    // 발사 주기 변수
    protected WaitForSeconds fireCycle = new WaitForSeconds(1f);


    // 넉백의 지속 시간
    private WaitForSeconds KnockbackTime = new WaitForSeconds(0.3f);


    private Transform playerTransform;
    private Rigidbody2D enemyRigid;


    // 풀에 다시 넣을건지를 판단하는 bool
    private bool IsPoolInsert = false;

    private void Start()
    {
        enemyRigid = GetComponent<Rigidbody2D>();
        playerTransform = GameManager.Instance.PlayerTransform;
        Init();
    }

    // 상속받은 곳에서 사용할 것
    protected virtual void Init()
    {
        stats = new Stats(5, 1, 1f);
        IsPoolInsert = true;
    }


    private void FixedUpdate()
    {
        // 플레이어 방향으로 움직임 계산
        Vector2 lookingVector = (playerTransform.position - transform.position).normalized * Time.fixedDeltaTime;
        enemyRigid.MovePosition(enemyRigid.position + lookingVector * stats.Speed);
    }

    private void Update()
    {
        // 총알 위치는 플레이어 방향
        Vector2 gunDir = (playerTransform.position - transform.position).normalized;

        // 총알이 왼쪽으로 넘어가면 왼쪽으로 뒤집어 주기
        float flipX = (gunDir.x <= 0) ? -1f : 1f;
        enemyGun.transform.parent.localScale *= new Vector2(flipX, 1f);
        transform.localScale *= new Vector2(flipX, 1f);

        // 총이 플레이어 방향으로 회전하기
        float z = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;
        enemyGun.transform.parent.rotation = Quaternion.Euler(0f, 0f, z);
    }


    // 몬스터가 소환되는 함수, 몬스터의 스펙을 초기화 하는 함수, 총으로 공격함수도 실행
    public void SetPosition(Vector2 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        Init();
        enemyGun.Init();
        enemyGun.BulletFire();
    }


    // 몬스터가 사라지면 풀에 다시 넣고 스포너의 카운터 --
    private void OnDisable()
    {
        if (IsPoolInsert)
        {
            IsPoolInsert = false;
            PoolManager.Instance.InsertObject("Enemy", gameObject);
        }
    }

    // 공격을 받는 함수
    public void BeAttacked(int damage)
    {
        stats.Health -= damage;
        if (stats.Health <= 0)
        {
            gameObject.SetActive(false);
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
