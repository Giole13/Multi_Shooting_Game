using UnityEngine;
using UnityEngine.SceneManagement;

// 플레이어의 기본정보 클래스 
public class Player : MonoBehaviour, IDamageable
{
    public Stats stats;

    private void Awake()
    {
        stats = new Stats(5, 1, 10f);
    }

    // 공격을 받는 함수
    public virtual void BeAttacked(int damage)
    {
        stats.Health -= damage;
        GameManager.Instance.PlayerStatsUI.DecreasePlayerHp();
        if (stats.Health <= 0)
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameManager.Instance.SceneMove(Define.ENDING_SCENE_NAME);
        }
    }


}
