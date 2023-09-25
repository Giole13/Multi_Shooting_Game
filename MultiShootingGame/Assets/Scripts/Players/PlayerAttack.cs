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
    }

    void Update()
    {
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

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
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

        // 멀티플레이인 경우
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
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(weaponPointTransform.rotation.eulerAngles.z);
            stream.SendNext(weaponPointTransform.localScale.x);
        }
        else
        {
            weaponPointTransform.rotation = Quaternion.Euler(0f, 0f, (float)stream.ReceiveNext());
            weaponPointTransform.localScale = new Vector3((float)stream.ReceiveNext(), 1f, 0f);
        }

        // var tr = transform;

        // // Write
        // if (stream.IsWriting)
        // {
        //     if (this.m_SynchronizePosition)
        //     {
        //         if (m_UseLocal)
        //         {
        //             this.m_Direction = tr.localPosition - this.m_StoredPosition;
        //             this.m_StoredPosition = tr.localPosition;
        //             stream.SendNext(tr.localPosition);
        //             stream.SendNext(this.m_Direction);
        //         }
        //         else
        //         {
        //             this.m_Direction = tr.position - this.m_StoredPosition;
        //             this.m_StoredPosition = tr.position;
        //             stream.SendNext(tr.position);
        //             stream.SendNext(this.m_Direction);
        //         }
        //     }

        //     if (this.m_SynchronizeRotation)
        //     {
        //         if (m_UseLocal)
        //         {
        //             stream.SendNext(tr.localRotation);
        //         }
        //         else
        //         {
        //             stream.SendNext(tr.rotation);
        //         }
        //     }

        //     if (this.m_SynchronizeScale)
        //     {
        //         stream.SendNext(tr.localScale);
        //     }
        // }
        // // Read
        // else
        // {
        //     if (this.m_SynchronizePosition)
        //     {
        //         this.m_NetworkPosition = (Vector3)stream.ReceiveNext();
        //         this.m_Direction = (Vector3)stream.ReceiveNext();

        //         if (m_firstTake)
        //         {
        //             if (m_UseLocal)
        //                 tr.localPosition = this.m_NetworkPosition;
        //             else
        //                 tr.position = this.m_NetworkPosition;

        //             this.m_Distance = 0f;
        //         }
        //         else
        //         {
        //             float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        //             this.m_NetworkPosition += this.m_Direction * lag;
        //             if (m_UseLocal)
        //             {
        //                 this.m_Distance = Vector3.Distance(tr.localPosition, this.m_NetworkPosition);
        //             }
        //             else
        //             {
        //                 this.m_Distance = Vector3.Distance(tr.position, this.m_NetworkPosition);
        //             }
        //         }

        //     }

        //     if (this.m_SynchronizeRotation)
        //     {
        //         this.m_NetworkRotation = (Quaternion)stream.ReceiveNext();

        //         if (m_firstTake)
        //         {
        //             this.m_Angle = 0f;

        //             if (m_UseLocal)
        //             {
        //                 tr.localRotation = this.m_NetworkRotation;
        //             }
        //             else
        //             {
        //                 tr.rotation = this.m_NetworkRotation;
        //             }
        //         }
        //         else
        //         {
        //             if (m_UseLocal)
        //             {
        //                 this.m_Angle = Quaternion.Angle(tr.localRotation, this.m_NetworkRotation);
        //             }
        //             else
        //             {
        //                 this.m_Angle = Quaternion.Angle(tr.rotation, this.m_NetworkRotation);
        //             }
        //         }
        //     }

        //     if (this.m_SynchronizeScale)
        //     {
        //         tr.localScale = (Vector3)stream.ReceiveNext();
        //     }

        //     if (m_firstTake)
        //     {
        //         m_firstTake = false;
        //     }
        // }
    }
}