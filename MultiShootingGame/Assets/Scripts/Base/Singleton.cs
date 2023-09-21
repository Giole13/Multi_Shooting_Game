using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 싱글톤 클래스
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
                _instance = FindObjectOfType<T>();
                if (_instance is null)
                {
                    // 자신의 스크립트 이름으로 객체 생성
                    GameObject obj = new GameObject(typeof(T).ToString());
                    // 해당 객체에 자신 스크립트 추가
                    _instance = obj.AddComponent<T>();

                    // DontDestroyOnLoad(obj);
                }
                // DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    // 초기화 함수
    public virtual void Init() { }
}
