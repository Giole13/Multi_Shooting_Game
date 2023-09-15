using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 엔딩에서 오브젝트의 생명주기를 책임지는 클래스
public class ObjectManager : MonoBehaviour
{
    private void Awake()
    {
        // PoolManager 삭제
        Destroy(FindObjectOfType<PoolManager>().gameObject);
    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
