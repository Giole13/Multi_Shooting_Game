using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 피격을 받아 체력이 줄어들 수 있는 인터페이스
public interface IDamageable
{
    public void BeAttacked(int damage);
}


// 공격 구현
public interface IAttack
{

}



// 위치를 지정해줘야하는 인터페이스
public interface ISetPosition
{
    public void SetPosition(Vector2 pos);
}


// 총알의 움직임을 구현
public interface IBullet
{
    // 총알이 나가는 방향, 위치, 데미지
    public void ShottingBullet(Vector2 dir, Vector2 pos, int damage);
}
