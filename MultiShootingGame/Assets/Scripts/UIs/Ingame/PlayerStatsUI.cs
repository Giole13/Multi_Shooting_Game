using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 플레이어의 UI를 책임지는 클래스
public class PlayerStatsUI : MonoBehaviour
{
    private Stack<GameObject> hpStack = new Stack<GameObject>();

    [SerializeField] private GameObject hpObject;
    [SerializeField] private Transform hpContain;
    [SerializeField] private GameObject ammoObject;
    [SerializeField] private TMP_Text ammoText;

    private Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
        GameObject hpobj = hpObject;

        // hp 스택을 플레이어 hp 만큼 초기화
        for (int i = 0; i < player.stats.Health; i++)
        {
            hpobj = Instantiate(hpObject, hpContain);
            hpobj.SetActive(true);
            hpStack.Push(hpobj);
        }
        // 마지막 hp를 캐싱
        hpObject = hpobj;
    }

    // 플레이어의 체력을 깍는걸 보여주는 함수
    public void DecreasePlayerHp()
    {
        // 마지막 오브젝트를 꺼내와서 꺼주기
        hpObject = hpStack.Pop();
        hpObject.SetActive(false);
    }

    public void IncreasePlayerHp()
    {
        // 마지막 hp오브젝트를 넣고 켜주기
        hpStack.Push(hpObject);
        hpObject.SetActive(true);
    }

    public void SetAmmoTxet(int currentAmmo, int maxAmmo)
    {
        ammoText.SetText($"{currentAmmo}/{maxAmmo}");
    }
}
