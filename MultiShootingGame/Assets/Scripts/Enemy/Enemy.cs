using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviourPun, ISetPosition, IDamageable, IPunObservable
{
    [SerializeField] protected Gun enemyGun;

    [SerializeField] Transform weaponPointTransform;

    [SerializeField] Quaternion networkRotation;

    [SerializeField] protected float bulletSpeed = 10;

    protected Stats stats;
    protected IBullet bullet;
    protected Vector2 bulletDir;

    // 발사 주기 변수
    protected WaitForSeconds fireCycle = new WaitForSeconds(1f);


    // 넉백의 지속 시간
    protected WaitForSeconds KnockbackTime = new WaitForSeconds(0.3f);


    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private Transform targetTransform;
    private Rigidbody2D enemyRigid;

    [SerializeField] private float enemyScale;


    // 풀에 다시 넣을건지를 판단하는 bool
    protected bool IsPoolInsert = false;

    // 공격을 할 수 있게 허락하는 변수
    // private bool IsAttack;

    private void Start()
    {
        enemyRigid = GetComponent<Rigidbody2D>();
        // targetTransform = GameManager.Instance.PlayerTransform;
        Init();
        // IsAttack = false;

        // 멀티 : 자신이 마스터에서 관리하는 객체가 아닐 경우
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay)
        {
            gameObject.SetActive(false);
        }
        // gameObject.SetActive(false);
    }

    // 상속받은 곳에서 사용할 것
    // 적의 정보들을 초기화하는 함수
    protected virtual void Init()
    {
        stats = new Stats(5, 1, 1f);
        IsPoolInsert = true;
        enemyScale = 1f;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient == false && GameManager.Instance.IsMultiPlay)
        {
            // 지연보상으로 현재 거리와 동기화 된 거리를 보간한다.
            weaponPointTransform.rotation = Quaternion.RotateTowards(weaponPointTransform.rotation,
                                                    networkRotation, 1000f);
            return;
        }

        // 목표 방향으로 움직임 계산
        Vector2 lookingVector = (targetTransform.position - transform.position).normalized * Time.fixedDeltaTime;
        enemyRigid.MovePosition(enemyRigid.position + lookingVector * stats.Speed);
    }

    private void Update()
    {
        // 마스터만 접근 가능함
        if (PhotonNetwork.IsMasterClient == false && GameManager.Instance.IsMultiPlay)
        {
            return;
        }

        // 총알 위치는 플레이어 방향
        Vector2 gunDir = (targetTransform.position - transform.position).normalized;

        // 총알이 왼쪽으로 넘어가면 왼쪽으로 뒤집어 주기
        float flipX = (gunDir.x <= 0) ? -1f : 1f;
        enemyGun.transform.parent.localScale = new Vector2(flipX, 1f);
        transform.localScale = new Vector2(flipX, 1f) * enemyScale;

        // 총이 플레이어 방향으로 회전하기
        float z = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;
        enemyGun.transform.parent.rotation = Quaternion.Euler(0f, 0f, z);
    }


    // 아래쪽 함수들은 리펙토링 예정

    public void SetPosition(Vector2 pos)
    {
        // 싱글 : 로컬일 경우
        if (GameManager.Instance.IsMultiPlay == false)
        {
            SetEnemySpawn(pos);

            StartCoroutine(SearchingTargetToDistanceCompare());
            return;
        }

        // 멀티 : 마스터일 경우 
        if (PhotonNetwork.IsMasterClient && GameManager.Instance.IsMultiPlay)
        {
            // 상세 설정은 전역으로 설정한다.
            photonView.RPC("SetEnemySpawn", RpcTarget.All, pos);

            // 객체가 활성화가 되어야 작동하는 코루틴
            StartCoroutine(SearchingTargetToDistanceCompare());
            return;
        }
    }

    // 적이 스폰될 때 초기화하는 함수
    [PunRPC]
    protected void SetEnemySpawn(Vector2 position)
    {
        TryGetComponent(out spriteRenderer);
        originalColor = spriteRenderer.color;

        Init();
        gameObject.SetActive(true);
        transform.position = position;
        enemyGun.SettingGun();
        enemyGun.Init();
        enemyGun.BulletFire();
    }

    private IEnumerator SearchingTargetToDistanceCompare()
    {
        // 싱글 : 플레이어가 한명이라서 바로 참조 가능 및 코루틴 탈출 
        if (GameManager.Instance.IsMultiPlay == false)
        {
            targetTransform = GameManager.Instance.PlayerTransform;
            yield break;
        }

        float targetDistanceFloat = float.MaxValue;

        while (true)
        {
            // 가장 가까운 플레이어를 계산해서 ViewID 값을 계산한다.
            foreach (var target in GameManager.Instance.PlayerDictionary)
            {
                // 해당 오브젝트가 비활성화 상태일 경우 다른 객체를 참조하고 패스
                if (target.Value.activeSelf == false)
                {
                    targetDistanceFloat = float.MaxValue;
                    continue;
                }

                // 만약 비교한 거리보다 작다면 더욱 가깝다 생각
                if ((transform.position - target.Value.transform.position).sqrMagnitude < targetDistanceFloat)
                {
                    // 적과의 거리 계산 및 타겟팅
                    targetDistanceFloat = (transform.position - target.Value.transform.position).sqrMagnitude;
                    targetTransform = GameManager.Instance.PlayerDictionary[target.Key].transform;
                }
            }
            yield return new WaitForSeconds(1f);
            // 1초마다 갱신
            targetDistanceFloat = (transform.position - targetTransform.position).sqrMagnitude;
        }
    }


    // 공격을 받는 함수
    public virtual void BeAttacked(int damage)
    {
        stats.Health -= damage;
        // 체력이 0 이하가 됬을 때 몬스터는 사라진다.
        if (stats.Health <= 0)
        {
            gameObject.SetActive(false);

            // 몬스터는 풀에 넣어주기
            if (IsPoolInsert)
            {
                IsPoolInsert = false;
                PoolManager.Instance.InsertObject("Enemy", gameObject);
            }
            return;
        }
        StartCoroutine(Knockback());
    }

    protected bool IsKnockback = false;
    // 피격당해 넉백하는 코루틴
    protected IEnumerator Knockback()
    {
        spriteRenderer.color = Color.white;
        // 넉백중이면 코루틴 취소
        if (IsKnockback)
        {
            spriteRenderer.color = originalColor;
            yield break;
        }
        stats.Speed *= -1;
        IsKnockback = true;
        yield return KnockbackTime;
        IsKnockback = false;
        stats.Speed *= -1;

        spriteRenderer.color = originalColor;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(weaponPointTransform.rotation);
            stream.SendNext(weaponPointTransform.localScale.x);
        }
        else
        {
            networkRotation = (Quaternion)stream.ReceiveNext();
            weaponPointTransform.localScale = new Vector3((float)stream.ReceiveNext(), 1f, 0f);
        }
    }

}
