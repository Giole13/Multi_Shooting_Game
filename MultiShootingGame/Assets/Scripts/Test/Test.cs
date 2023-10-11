using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Test : MonoBehaviour
{
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        GameObject testObj = Instantiate(obj);


        StartCoroutine(TestCoroutine());

        IEnumerator TestCoroutine()
        {
            yield return new WaitForSeconds(1f);
            testObj.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
