using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    Archer
}

public interface ITower 
{
    void Shoot(Transform _Target);

    int Upgrade(int currentLevel);

    float GetShootCD();

    TowerData GetData();
}
