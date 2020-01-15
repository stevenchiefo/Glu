using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private UIPlayerHealth m_UIPlayerHealth;
    [SerializeField] private float m_Health = 6;
    [SerializeField] private Inventory[] m_Inventory = new Inventory[16];
    private PlayerMovement m_PlayerMovement;
    [SerializeField] public int Coins = 0;

    private void Start()
    {
        if (m_UIPlayerHealth == null)
        {
            Debug.LogError("there is no UiPlayerSelected");
        }
        m_Inventory[0] = new Inventory(new Potion("HealthPotion", Item.KindItem.Potion, 20));
    }

    // Update is called once per frame
    private void Update()
    {
        Mechanics();
        Death();
    }

    private void Mechanics()
    {
        UseItem();
    }

    private void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_Health += m_Inventory[0].Item.UseItem();
        }
    }

    private void Death()
    {
        if (m_Health <= 0)
        {
            Debug.Log("Player Died");
        }
    }

    public void GotHit()
    {
        m_Health -= 1;
        m_UIPlayerHealth.TookDamage();
    }

    private void LoadPlayer()
    {
        m_PlayerMovement = gameObject.GetComponent<PlayerMovement>();
    }
}