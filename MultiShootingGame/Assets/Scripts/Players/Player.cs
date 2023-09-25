using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

// 플레이어의 기본정보 클래스 
public class Player : MonoBehaviourPun, IDamageable
{
    public Stats stats;
    public Vector2 bulletDir
    {
        get; protected set;
    }

    private void Awake()
    {
        Init();
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

        // 2023.09.20 / HyungJun / 디버그용 주석 - 플레이어 무적
        stats.Health -= damage;
        GameManager.Instance.PlayerStatsUI.DecreasePlayerHp();
        if (stats.Health <= 0)
        {
            GameManager.Instance.SceneMove(Define.ENDING_SCENE_NAME);
        }
    }


}
