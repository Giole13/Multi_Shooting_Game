using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

// 플레이어의 마우스 인풋을 책임지는 클래스
public class PlayerAim : MonoBehaviour
{
    private Vector2 mousePosition;
    [SerializeField] private Transform Weapon;
    [SerializeField] private GameObject bulletObj;
    private Vector2 bulletDir;
    private IBullet bullet;
    private int damage;

    private void Start()
    {
        damage = 10;
        // bullet = bulletObj.GetComponent<IBullet>();
    }

    void Update()
    {
        // 마우스 입력을 월드 좌표로 계산 및 방향 구하기
        Vector2 len = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;

        // 총알이 나가는 방향을 계산
        bulletDir = len.normalized;

        // 플레이어의 바라보는 방향이 0보다 작으면 왼쪽
        if (len.x <= 0)
        {
            Weapon.localScale = new Vector2(-1f, 1f);
            transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            Weapon.localScale = new Vector2(1f, 1f);
            transform.localScale = new Vector2(1f, 1f);
        }

        // 회전값 계산
        float z = Mathf.Atan2(len.y, len.x) * Mathf.Rad2Deg;
        Weapon.rotation = Quaternion.Euler(0f, 0f, z);
    }

    private void OnAim(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }

    // 좌클릭 함수
    private void OnAttack()
    {
        GameObject obj = Instantiate(bulletObj);
        bullet = obj.GetComponent<IBullet>();

        // 슈팅만 하면 됨
        bullet.ShottingBullet(bulletDir, transform.position, damage);

    }

    private void OnSubSkill()
    {
        Debug.Log("보조 스킬!");
    }
}
