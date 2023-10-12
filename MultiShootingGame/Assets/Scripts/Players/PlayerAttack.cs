using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

// 플레이어의 공격을 책임지는 클래스
public class PlayerAttack : MonoBehaviourPun, IGunFirstAcquisition, IPunObservable
{
    [SerializeField] private Gun playerBaseGun;

    private IGun playerGun;
    private PlayerInputHandler playerInputHandler;


    [SerializeField] private Transform weaponPointTransform;
    [SerializeField] private Transform playerSkillTransform;

    private Quaternion networkRotation;

    public Vector2 bulletDir
    {
        get; private set;
    }

    private Queue<Transform> weaponQueue;

    private bool IsFire;

    private void Awake()
    {
        TryGetComponent<PlayerInputHandler>(out playerInputHandler);
        weaponQueue = new Queue<Transform>();
    }
    private void Start()
    {
        // 플레이어 정보 초기화
        playerGun = playerBaseGun;
        playerGun.Init();
        IsFire = false;
        IsSkillActive = false;

        playerSkillTransform.gameObject.SetActive(false);

        // 자신이 아닌 객체라면
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay)
        {
            playerBaseGun.MakeItNonLocal();
            return;
        }

        // 로컬의 총 UI 갱신
        playerBaseGun.UpdateAmmoUI();
    }

    private void FixedUpdate()
    {
        // 자신이 아닌 객체라면
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay)
        {
            // 지연보상으로 현재 거리와 동기화 된 거리를 보간한다.
            weaponPointTransform.rotation = Quaternion.RotateTowards(weaponPointTransform.rotation,
                                                    networkRotation, 1000f);
            return;
        }

        // 플레이어의 바라보는 방향이 0보다 작으면 왼쪽
        float flipX = (playerInputHandler.bulletDir.x <= 0) ? -1f : 1f;
        weaponPointTransform.localScale = new Vector2(flipX, 1f);
        transform.localScale = new Vector2(flipX, 1f);

        // 회전값 계산
        float z = Mathf.Atan2(playerInputHandler.bulletDir.y, playerInputHandler.bulletDir.x) * Mathf.Rad2Deg;
        weaponPointTransform.rotation = Quaternion.Euler(0f, 0f, z);
    }

    // 좌클릭 함수
    private void OnAttack()
    {
        // 자신이 아닌 객체는 리턴
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

        // 자신의 멀티플레이인 경우
        if (GameManager.Instance.IsMultiPlay)
        {
            photonView.RPC("FireBullet", RpcTarget.All);
            return;
        }

        // 싱글플레이인 경우
        FireBullet();
    }

    [PunRPC]
    private void FireBullet()
    {
        // 발사중이 아니면 발사
        if (IsFire == false)
        {
            IsFire = true;
            // 그냥 총의 기능만 사용하면 됨
            playerGun.BulletFire();
        }
        // 발사하다가 좌클릭을 때면 공격 중단
        else if (IsFire)
        {
            IsFire = false;
            playerGun.BulletFireStop();
            return;
        }
    }

    private bool IsSkillActive;

    // 스킬을 사용하는 함수
    private void OnSubSkill()
    {
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

        StartCoroutine(SkillActive());
        IEnumerator SkillActive()
        {
            // 스킬이 활성화 되어있다면 스킬 사용 불가능
            if (IsSkillActive)
            {
                yield break;
            }
            IsSkillActive = true;
            playerSkillTransform.gameObject.SetActive(true);
            float scale = 0.1f;
            while (true)
            {
                playerSkillTransform.localScale = new Vector2(scale, scale);
                scale += 0.3f;
                yield return null;
                // 스킬의 한 사이클
                if (45f <= scale)
                {
                    playerSkillTransform.gameObject.SetActive(false);
                    IsSkillActive = false;
                    yield break;
                }
            }
        }
    }



    // 가지고 있는 총을 스왑하는 함수
    private void OnGunChange()
    {
        // 멀티플레이인 경우
        if (GameManager.Instance.IsMultiPlay)
        {
            photonView.RPC("ChangeWeapon", RpcTarget.All);
            return;
        }

        ChangeWeapon();
    }

    // 총을 최초로 획득하면 해당 총으로 바꾸고 값을 초기화한다.
    public void FirstAcquisitionChangeWeapon(Transform gunTransform)
    {
        weaponQueue.Enqueue(gunTransform);

        // 가지고 있는 총의 상위 객체로 위치를 옮긴다.
        // 이후 총의 초기화 함수 호출
        gunTransform.SetParent(playerBaseGun.transform.parent);

        for (int i = 0; i < weaponQueue.Count; i++)
        {
            ChangeWeapon();
        }
    }

    // 무기를 획득했을때 실행하는 함수
    [PunRPC]
    private void ChangeWeapon()
    {
        playerBaseGun.enabled = false;
        playerBaseGun.gameObject.SetActive(false);

        weaponQueue.Enqueue(playerBaseGun.transform);
        weaponQueue.Dequeue().TryGetComponent<Gun>(out playerBaseGun);
        playerBaseGun.TryGetComponent<IGun>(out playerGun);

        playerBaseGun.enabled = true;
        playerBaseGun.gameObject.SetActive(true);

        playerGun.Init();

        // 자신이 아닌 객체라면
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay)
        {
            playerBaseGun.MakeItNonLocal();
            return;
        }

        // 로컬인 경우 UI 갱신
        playerBaseGun.UpdateAmmoUI();
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