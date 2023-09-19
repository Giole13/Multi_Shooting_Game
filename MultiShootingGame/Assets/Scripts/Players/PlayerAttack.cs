using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

// 플레이어의 공격을 책임지는 클래스
public class PlayerAttack : MonoBehaviour, IGunChangeable
{
    [SerializeField] private Gun playerBaseGun;
    private IGun playerGun;

    private Player player;
    private PlayerInputHandler playerInputHandler;

    [SerializeField] private Transform WeaponPointTransform;
    public Vector2 bulletDir
    {
        get; private set;
    }

    private bool IsFire;

    private void Awake()
    {
        TryGetComponent<PlayerInputHandler>(out playerInputHandler);
    }
    private void Start()
    {
        // 플레이어 정보 초기화
        player = GameManager.Instance.Player;
        playerGun = playerBaseGun;
        playerGun.Init(Define.PLAYER_BULLET_NAME);
        IsFire = false;
    }

    void Update()
    {
        // 플레이어의 바라보는 방향이 0보다 작으면 왼쪽
        float flipX = (playerInputHandler.bulletDir.x <= 0) ? -1f : 1f;
        WeaponPointTransform.localScale = new Vector2(flipX, 1f);
        transform.localScale = new Vector2(flipX, 1f);

        // 회전값 계산
        float z = Mathf.Atan2(playerInputHandler.bulletDir.y, playerInputHandler.bulletDir.x) * Mathf.Rad2Deg;
        WeaponPointTransform.rotation = Quaternion.Euler(0f, 0f, z);
    }


    // 좌클릭 함수
    private void OnAttack()
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

    // 우클릭 함수
    private void OnSubSkill()
    {
        Debug.Log("보조 스킬!");
    }

    // 총을 획득하면 해당 총으로 바꾼다.
    public void ChangeWeapon(Transform gunTransform)
    {
        playerBaseGun.enabled = false;
        playerBaseGun.gameObject.SetActive(false);

        // 가지고 있는 총의 상위 객체로 위치를 옮긴다.
        // 이후 총의 초기화 함수 호출
        gunTransform.SetParent(playerBaseGun.transform.parent);
        gunTransform.TryGetComponent<Gun>(out playerBaseGun);
        gunTransform.TryGetComponent<IGun>(out playerGun);
        playerGun.Init(Define.PLAYER_BULLET_NAME);

    }
}
