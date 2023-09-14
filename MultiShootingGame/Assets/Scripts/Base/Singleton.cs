using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            // 널인 경우 새로운 객체 생성 및 반환
            if (_instance is null)
            {
                // 자신의 스크립트 이름으로 객체 생성
                GameObject newObject = new GameObject(typeof(T).ToString());
                // 해당 객체에 자신 스크립트 추가
                _instance = newObject.AddComponent<T>();
                // 해당 객체를 DontDestory 객체로 변환
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
}
