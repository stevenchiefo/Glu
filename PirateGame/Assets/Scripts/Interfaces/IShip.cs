using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    LeftSide,
    RightSide,
    Forward,
}

public interface IShip
{
    int Durrability { get; set; }

    void TakeDamage(int _Damage);

    void Rotate(Vector2 vector2);

    void Move(Vector3 vector3);

    void Shoot(AttackType attackType);

    CannonBall.TargetType GetTargetType();

    (Vector3 ForwardShoot, Vector3 SideRightWaysShoot, Vector3 SideLeftWaysShoot) GetShootingPoint();

    Rigidbody GetRB();
}