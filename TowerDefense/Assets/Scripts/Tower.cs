using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using TMPro;
using UnityEngine;

public class Tower : PoolableObject
{

    private List<IEnemy> m_Enemys;
    [SerializeField] private Transform m_FirePoint;

    public int Level { get; private set; }
    public ITower m_Tower;

    private bool DoShoot;
    private Collider m_Collider;
    private void LoadCom()
    {
        m_Tower = GetComponent<ITower>();
        m_Enemys = new List<IEnemy>();
        OnPool.AddListener(StopCo);
        m_Collider = GetComponent<Collider>();
    }
    protected Transform GetFirePoint()
    {
        return m_FirePoint;
    }

    private void StopCo()
    {
        StopAllCoroutines();
    }

    public void Upgrade()
    {
        Level = m_Tower.Upgrade(Level);
    }

    public override void SpawnObject(Vector3 position)
    {
        base.SpawnObject(position);
        if (m_Tower == null)
        {
            LoadCom();
        }
        StartCoroutine(ShootOnCD());
    }
    private void Update()
    {
        if (m_Enemys != null)
        {

            DoShoot = m_Enemys.Count > 0;
        }
    }

    private IEnumerator ShootOnCD()
    {
        while (true)
        {


            yield return new WaitForSeconds(m_Tower.GetShootCD());
            yield return new WaitUntil(() => DoShoot == true);
            if (m_Enemys.Count > 0)
            {
                if (m_Enemys[0].IsAlive)
                {
                    if (Vector3.Distance(m_Enemys[0].GetTarget().position, m_FirePoint.position) <= m_Collider.bounds.size.x)
                    {



                        if (CanShootEnemy(m_Enemys[0]))
                        {
                            m_Tower.Shoot(m_Enemys[0].GetTarget());

                        }

                    }
                    else
                    {
                        m_Enemys.RemoveAt(0);
                    }
                }
                else
                {
                    m_Enemys.RemoveAt(0);
                }
            }
        }
    }

    private bool CanShootEnemy(IEnemy enemy)
    {
        if (m_Tower.GetData().CantShoot.Length > 0)
        {
            return m_Tower.GetData().CantShoot.Contains(enemy.EnemyType);

        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForEnemy(other);
    }

    private void CheckForEnemy(Collider collider)
    {
        IEnemy enemy = collider.GetComponentInParent<IEnemy>();
        if (enemy != null)
        {
            m_Enemys.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveEnemy(other);
    }
    private void RemoveEnemy(Collider collider)
    {
        IEnemy enemy = collider.GetComponentInParent<IEnemy>();
        if (enemy != null)
        {
            m_Enemys.Remove(enemy);
        }
    }
}
