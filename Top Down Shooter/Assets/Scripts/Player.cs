using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float m_Health = 100;
    [SerializeField] private Inventory[] m_Inventory = new Inventory[16];
    private PlayerMovement m_PlayerMovement;
    [SerializeField] public int Coins = 0;

    private void Start()
    {
        m_Inventory[0] = new Inventory(new Potion("HealthPotion", Item.KindItem.Potion, 20));
    }

    // Update is called once per frame
    private void Update()
    {
        Mechanics();
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

    public void GotHit(float Damage)
    {
        m_Health -= Damage;
    }

    private void LoadPlayer()
    {
        m_PlayerMovement = gameObject.GetComponent<PlayerMovement>();
    }
}