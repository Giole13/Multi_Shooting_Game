using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 엔딩에서 오브젝트의 생명주기를 책임지는 클래스
public class ObjectManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.Init();
    }



}
