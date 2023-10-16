using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

// 플레이어의 기본정보 클래스 
public class Player : MonoBehaviourPun, IDamageable
{
    // 자신을 나타내는 삼각형
    [SerializeField] protected Transform selfIndication;


    public Stats stats;
    public Vector2 bulletDir
    {
        get; protected set;
    }

    private void Awake()
    {
        Init();
        selfIndication.gameObject.SetActive(false);

        // 플레이어를 캐싱하는 딕셔너리에다가 넣기
        GameManager.Instance.PlayerDictionary.Add(photonView.ViewID, gameObject);

        // 자기 자신이라면 자신을 표시하는 삼각형 켜주기
        if (photonView.IsMine)
        {
            selfIndication.gameObject.SetActive(true);
        }
    }

    private void Start() { }
    private void Update() { }

    // 자식에서 초기화하는 함수
    protected virtual void Init()
    {
        stats = new Stats(5, 1, 10f);
    }


    // 공격을 받는 함수
    public virtual void BeAttacked(int damage)
    {
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }


        // 멀티 : 각 클라이언트의 자신의 체력을 동기화
        if (GameManager.Instance.IsMultiPlay)
        {
            photonView.RPC("BeAttackedRPC", RpcTarget.All, damage);
        }
        // 싱글 : 체력만 소모
        else
        {
            stats.Health -= damage;
        }

        // 체력 UI 갱신
        GameManager.Instance.PlayerStatsUI.DecreasePlayerHp();
        // 피격시 흔들림 효과 추가
        GameManager.Instance.BeAttackedEffect();

        // return; // 디버그용

        // 체력이 0이 되면 엔딩 씬으로 이동
        if (stats.Health <= 0)
        {
            // 싱글 : 엔딩 씬으로 넘기기
            if (GameManager.Instance.IsMultiPlay == false)
            {
                gameObject.SetActive(false);
                GameManager.Instance.SceneMove(Define.ENDING_SCENE_NAME);
                return;
            }
            // 멀티 : 
            // 1. 플레이어 꺼주기
            // 2. 플레이어 생존수 감소
            // 3. 관전으로 변경
            // 4. 생존자 수가 0명 이하일 경우 엔딩으로 넘기기

            Debug.Assert(GameManager.Instance.PlayerDictionary.Count == 2);

            foreach (var obj in GameManager.Instance.PlayerDictionary)
            {
                Debug.Log(obj.Key);
                // 자신과 다른 viewID 값 -> 다른 플레이어를 찾았을 때
                if (photonView.ViewID != obj.Key)
                {
                    Debug.Log($"전환할 플레이어 아이디 : {obj.Key}");
                    GameManager.Instance.SwitchPlayerFollowingCamera(obj.Key);
                    break;
                }
            }
            photonView.RPC("DiePlayerRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    protected void DiePlayerRPC()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ReductionPlayerLiveCount();
        GameManager.Instance.PlayerDictionary.Remove(photonView.ViewID);
    }

    [PunRPC]
    protected void BeAttackedRPC(int damage)
    {
        stats.Health -= damage;
    }
}
