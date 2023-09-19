using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 피격을 받아 체력이 줄어들 수 있는 인터페이스
public interface IDamageable
{
    public void BeAttacked(int damage);
}

// 총을 바꿀 수 있는 인터페이스
public interface IGunChangeable
{
    public void ChangeWeapon(Transform gunTransform);
}



// 총을 구현
public interface IGun
{
    // 총알을 발사하는 함수
    public void BulletFire();

    // 공격을 멈추는 함수
    public void BulletFireStop();

    // 총을 획들했을 때 초기화 하는 함수, 사용할 총알의 이름을 매개변수로 받는다.
    public void Init(string bulletName);
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
