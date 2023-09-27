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

    // 자식에서 초기화하는 함수
    protected virtual void Init()
    {
        stats = new Stats(5, 1, 10f);
    }


    // 공격을 받는 함수
    public virtual void BeAttacked(int damage)
    {
        if (photonView.IsMine == false && GameManager.Instance.IsMultiPlay) { return; }

        // stats.Health -= damage;

        // // 로컬 플레이어라면 UI 갱신
        // if (photonView.IsMine)
        // {
        //     GameManager.Instance.PlayerStatsUI.DecreasePlayerHp();
        // }
        // else if (GameManager.Instance.IsMultiPlay)
        // {
        //     GameManager.Instance.PlayerStatsUI.DecreasePlayerHp();
        // }

        // // 체력이 0이 되면 엔딩 씬으로 이동
        // if (stats.Health <= 0)
        // {
        //     gameObject.SetActive(false);
        //     GameManager.Instance.SceneMove(Define.ENDING_SCENE_NAME);
        // }
    }
}
