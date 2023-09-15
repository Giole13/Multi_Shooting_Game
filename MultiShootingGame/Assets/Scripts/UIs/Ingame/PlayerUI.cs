using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 UI를 책임지는 클래스
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private List<Transform> hpList;

    private Player player;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    // 플레이어의 체력을 깍는걸 보여주는 함수
    public void DecreasePlayerHp()
    {

    }



}
