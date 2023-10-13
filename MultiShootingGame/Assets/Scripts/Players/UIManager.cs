using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

// 플레이어의 UI를 책임지는 클래스
public class UIManager : MonoBehaviour
{
    private Stack<GameObject> hpStack = new Stack<GameObject>();

    [SerializeField] private GameObject hpObject;
    [SerializeField] private Transform hpContain;
    [SerializeField] private TMP_Text ammoText;

    [SerializeField] private TMP_Text isHostTxt;


    private Player player;

    private void Start()
    {
        // 멀티플레이의 경우 해당 호스트 인지 게스트 인지 알려준다.
        if (GameManager.Instance.IsMultiPlay)
        {
            // 마스터의 경우
            if (PhotonNetwork.IsMasterClient)
            {
                isHostTxt.text = "Host";
            }
            else
            {
                isHostTxt.text = "Guest";
            }
        }
        // 싱글플레이의 경우 멀티 표시 끄기
        else if (GameManager.Instance.IsMultiPlay == false)
        {
            isHostTxt.gameObject.SetActive(false);
        }

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

    public void SetAmmoTxet(int currentAmmo, int maxAmmo, bool isUnlimitedBullets = false)
    {
        if (isUnlimitedBullets)
        {
            ammoText.SetText("∞");
            ammoText.fontSize = 100f;
            return;
        }
        ammoText.SetText($"{currentAmmo}/{maxAmmo}");
        ammoText.fontSize = 50f;
    }
}
