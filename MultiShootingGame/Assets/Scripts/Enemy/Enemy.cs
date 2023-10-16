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
    [SerializeField] private GameObject ammoPack;


    protected Stats stats;
    protected IBullet bullet;
    protected Vector2 bulletDir;

    // 발사 주기 변수
    protected WaitForSeconds fireCycle = new WaitForSeconds(1f);

    // 넉백의 지속 시간
    protected WaitForSeconds knockbackTime = new WaitForSeconds(0.3f);
    // 적을 찾는 주기
    protected WaitForSeconds searchTargetTime = new WaitForSeconds(1f);


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
    }

    // 상속받은 곳에서 사용할 것
    // 적의 정보들을 초기화하는 함수
    protected virtual void Init()
    {
        stats = new Stats(6, 1, 1f);
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
        Init();
        gameObject.SetActive(true);
        transform.position = position;
        enemyGun.SettingGun();
        enemyGun.Init();
        enemyGun.BulletFire();

        TryGetComponent(out spriteRenderer);
        originalColor = spriteRenderer.color;
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
            // 처음 한번 실행
            // 타겟 플레이어가 비활성화 되었을 때 실행
            if (targetDistanceFloat == float.MaxValue)
            {
                foreach (var target in GameManager.Instance.PlayerDictionary)
                {
                    // 자신과 가까운 적을 타겟팅 한다
                    if ((transform.position - target.Value.transform.position).sqrMagnitude < targetDistanceFloat)
                    {
                        targetDistanceFloat = (transform.position - target.Value.transform.position).sqrMagnitude;
                        targetTransform = GameManager.Instance.PlayerDictionary[target.Key].transform;
                        Debug.Log("타겟팅 완료");
                    }
                }
            }

            if (targetTransform.gameObject.activeSelf == false)
            {
                targetDistanceFloat = float.MaxValue;
            }

            yield return searchTargetTime;
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
            spriteRenderer.color = originalColor;

            // 몬스터는 풀에 넣어주기
            if (IsPoolInsert)
            {
                IsPoolInsert = false;
                PoolManager.Instance.InsertObject("Enemy", gameObject);
                // 탄약 팩 드랍
                Instantiate(ammoPack, transform.position, Quaternion.identity);
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
        yield return knockbackTime;
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
