using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public static Finish Instance;
    [SerializeField] private int m_MaxHealth;
    private int m_Health;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        m_Health = m_MaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForEnemy(other);
    }

    public (int currentHealth, int MaxHealth) GetInfo()
    {
        return (m_Health, m_MaxHealth);
    }

    private void CheckForEnemy(Collider collider)
    {
        IEnemy enemy = collider.GetComponentInParent<IEnemy>();
        if (enemy != null)
        {
            m_Health -= enemy.GetDamage();
            if (m_Health <= 0)
            {
                GameManager.Instance.SetGameOver();
            }
            enemy.IsAlive = false;
            collider.GetComponentInParent<PoolableObject>().PoolObject();
        }

        UIManager.Instance.UpdateUI();
    }
}