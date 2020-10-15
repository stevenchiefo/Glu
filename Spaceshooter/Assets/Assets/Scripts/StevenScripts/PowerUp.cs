using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PoolableObject
{
    [SerializeField] private AudioClip m_AudioClip;
    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckForPlayer(collision);
    }

    private void Update()
    {
        if (!BoundaryManager.Instance.IsAboveBottem(transform.position))
        {
            PoolObject();
        }
    }

    private void CheckForPlayer(Collider2D collider)
    {
        ShipController shipController = collider.GetComponent<ShipController>();
        if (shipController != null)
        {
            shipController.AddUpgrade();
            SoundPlayerPool.Instance.PlayAudio(m_AudioClip,transform.position);
            PoolObject();
        }
    }
}
