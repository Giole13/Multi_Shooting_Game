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


    private Transform targetTransform;
    private Rigidbody2D enemyRigid;


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
    protected virtual void Init()
    {
        stats = new Stats(5, 1, 1f);
        IsPoolInsert = true;
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
        enemyGun.transform.parent.localScale *= new Vector2(flipX, 1f);
        transform.localScale *= new Vector2(flipX, 1f);

        // 총이 플레이어 방향으로 회전하기
        float z = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;
        enemyGun.transform.parent.rotation = Quaternion.Euler(0f, 0f, z);
    }


    // 멀티 : 호스트 실행 - 몬스터가 소환되는 함수, 몬스터의 스펙을 초기화 하는 함수, 총으로 공격함수도 실행
    public void SetPosition(Vector2 pos)
    {
        int viewID = 0;

        // 싱글 : 로컬일 경우
        if (GameManager.Instance.IsMultiPlay == false)
        {
            SetEnemySpawn(pos, viewID);
            return;
        }

        // 멀티 : 마스터일 경우 
        if (PhotonNetwork.IsMasterClient && GameManager.Instance.IsMultiPlay)
        {
            // 호스트에서 몬스터가 추적할 플레이어를 연산 한다.
            viewID = SearchingTargetObject();
            photonView.RPC("SetEnemySpawn", RpcTarget.All, pos, viewID);
            return;
        }
    }

    [PunRPC]
    public void SetEnemySpawn(Vector2 position, int viewID)
    {
        // 멀티플레이인 경우 viewID 를 받아와서 룸 커스텀 프로퍼티로 찾아 객체를 참조한다.
        if (GameManager.Instance.IsMultiPlay)
        {
            // ViewID값으로 미리 캐싱해놓은 딕셔너리에서 가져와 타겟팅한다.
            targetTransform = GameManager.Instance.PlayerDictionary[viewID].transform;
        }
        // 싱글 : 플레이어가 한명이라서 바로 참조 가능 
        else if (GameManager.Instance.IsMultiPlay == false)
        {
            targetTransform = GameManager.Instance.PlayerTransform;
        }

        Init();
        gameObject.SetActive(true);
        transform.position = position;
        enemyGun.SettingGun();
        enemyGun.Init();
        enemyGun.BulletFire();

        // Debug.Log($"마스터 여부 : {PhotonNetwork.IsMasterClient}, 멀티 여부 : {GameManager.Instance.IsMultiPlay}");
    }

    // 추적할 오브젝트를 찾는 함수
    private int SearchingTargetObject()
    {
        #region 레거시 코드
        // // 식별할 레이어 마스크의 넘버 (2진수 형태가 아님)
        // LayerMask playerLayer = LayerMask.NameToLayer("Player");
        // // 2D 콜라이더에서 IsTrigger가 활성화 된 객체도 충돌을 허락하는 프로퍼티
        // Physics2D.queriesHitTriggers = true;

        // Collider2D[] targettargetCollider2dArray = new Collider2D[0];

        // // 1초 간격으로 플레이어를 계속 탐색
        // WaitForSeconds searchCycleTime = new WaitForSeconds(1f);

        // StartCoroutine(SearchCoroutine());
        // IEnumerator SearchCoroutine()
        // {
        //     while (true)
        //     {
        //         // 플레이어 레이어 만큼 2진수를 왼쪽으로 이동시키고 해당 레이어를 식별한다. -> 충돌된 객체들을 배열 형태로 반환
        //         targettargetCollider2dArray = Physics2D.OverlapCircleAll(transform.position, 10f, 1 << playerLayer);

        //         // 객체가 널이 아니게 되면
        //         if (targettargetCollider2dArray[0] != null)
        //         {
        //             break;
        //         }

        //         yield return searchCycleTime;
        //     }

        // }

        // // 비교할 거리를 담는 변수
        // float compareDistancefloat = float.MaxValue;

        // // 자신 (적)의 위치와 타켓의 위치를 비교하여 더 짧은 거리의 객체를 타겟으로 설정한다.
        // foreach (Collider2D targetCollider2d in targettargetCollider2dArray)
        // {
        //     // 만약 비교할 거리보다 작다면 해당 오브젝트를 목표 오브젝트로 설정한다.
        //     if ((targetCollider2d.transform.position - transform.position).sqrMagnitude <= compareDistancefloat)
        //     {
        //         targetTransform = targetCollider2d.transform;
        //     }
        //     // 만약 비교할 거리가 더 크다면 
        //     else
        //     {
        //         // 적과 자신의 거리를 대략적으로 구한다 (제곱값)
        //         compareDistancefloat = (targetCollider2d.transform.position - transform.position).sqrMagnitude;
        //     }
        // }

        // // 여기까지가 싱글기준으로 동일한 로직 -> 이 아래부터는 멀티 플레이 로직
        // // 만약 멀티환경에서 마스터 기준으로 로직을 실행한다.
        // if (PhotonNetwork.IsMasterClient && GameManager.Instance.IsMultiPlay)
        // {
        //     // 목표한 플레이어의 ID값을 반환한다.
        //     viewID = targetTransform.GetComponent<PhotonView>().ViewID;
        // }
        #endregion 레거시 코드

        Debug.Log($"플레이어의 ViewID 의 개수 : {GameManager.Instance.PlayerDictionary.Count}");

        // Ver.1 랜덤으로 찾아서 가져온다.
        // 1 ~ 2 의 숫자에 * 1000 + 1 = 플레이어 ViewID 값
        int viewID = (Random.Range(1, 3) * 1000) + 1;

        // 추후 버전 -> 멀티 : 커스텀 프로퍼티에서 캐싱된 플레이어들을 가져와서 직접 거리를 계산하고 짧은 쪽을 타켓으로 한다.

        return viewID;
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
