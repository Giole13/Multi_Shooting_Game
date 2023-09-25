using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 엔딩에서 오브젝트의 생명주기를 책임지는 클래스
public class ObjectManager : MonoBehaviour
{
    [SerializeField] private PhotonManager photonManager;

    private void Awake()
    {

        // 서버 나가기
        photonManager.LeaveServer();

        GameManager.Instance.Init();
        LoadingManager.Instance.Init();
    }



}
