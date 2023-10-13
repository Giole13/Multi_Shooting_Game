using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : Item
{
    void Start() { }
    void Update() { }

    // 해당 총의 탄약을 충전해준다.
    public override void AcquiredItem(Transform playerTransform)
    {
        gameObject.SetActive(false);
        // 모든 총을 가져온 후
        Gun[] gunTransforms = playerTransform.GetComponentsInChildren<Gun>();

        foreach (Gun target in gunTransforms)
        {
            // 활성화 된 총을 발견하면 총알을 중전해준다.
            if (target.gameObject.activeSelf)
            {
                target.SupplyGunAmmo();
                break;
            }
        }
    }

    // 플레이어와 충돌시 탄약 충전
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AcquiredItem(other.transform);
        }
    }
}
