using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임에서 사용되는 상수들을 모아놓은 클래스
public class Define : MonoBehaviour
{
    #region 씬의 이름
    public const string TITLE_SCENE_NAME = "01.Title";
    public const string LOADING_SCENE_NAME = "02.Loading";
    public const string SELECTCHARACTER_SCENE_NAME = "03_1.SelectCharacter";
    public const string INGAME_SCENE_NAME = "03_2.Ingame";
    public const string BOSS_SCENE_NAME = "03_3.Boss";
    public const string ENDING_SCENE_NAME = "05.Ending";
    #endregion 씬의 이름

    #region 총알의 이름
    public const string PLAYER_BULLET_NAME = "PlayerBullet";
    public const string ENEMY_BULLET_NAME = "EnemyBullet";

    #endregion 총알의 이름

}
