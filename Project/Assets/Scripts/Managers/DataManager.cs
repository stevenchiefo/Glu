using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public BulletData PlayerBulletData;
    public BulletData EnemyBulletData;
    public BulletData TankBustBulletData;
    public EnemyData EnemyData;
    public EnemyData TankBusterData;
    public MissleData PlayerMissleData;
    public Transform PlayerTrans;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        StartCoroutine(CheckLoop());
    }

    private IEnumerator CheckLoop()
    {
        while (true)
        {
            yield return new WaitUntil(() => PlayerTrans == null);
            PlayerTrans = FindObjectOfType<Player>().transform;
        }
    }
}