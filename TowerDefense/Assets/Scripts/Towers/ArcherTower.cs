using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class ArcherTower : Tower, ITower
{
    private ObjectPool m_ProjectilePool;

    private void Start()
    {
        Load();
        m_ProjectilePool = gameObject.AddComponent<ObjectPool>();
        m_ProjectilePool.BeginPool(DataBase.Instance.GetTowerData(TowerType.Archer).Projectile, 5, transform);
    }

    public float GetShootCD()
    {
        return DataBase.Instance.GetTowerData(TowerType.Archer).ShootCD;
    }

    public TowerData GetData()
    {
        return DataBase.Instance.GetTowerData(TowerType.Archer);
    }

    public void Shoot(Transform _Target)
    {
        PoolableObject poolableObject = m_ProjectilePool.GetObject();
        poolableObject.SpawnObject(GetFirePoint().position);
        poolableObject.LookAtTraget(_Target);

        IShootAble shootAble = poolableObject.GetComponent<IShootAble>();
        shootAble.FireObject(_Target.position - GetFirePoint().position);
    }


    public int Upgrade(int currentLevel)
    {
        if(currentLevel + 1 >= DataBase.Instance.GetTowerData(TowerType.Archer).MaxLevel)
        {
            return currentLevel;
        }
        return currentLevel++;
    }

    

    
}
