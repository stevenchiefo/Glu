using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    private Vector3 m_PlayersLastPos;
    private bool m_HasBeenOnIsland;
    private bool m_AlreadyHittingBoat;
    public int ID;

    private void Start()
    {
        m_HasBeenOnIsland = false;
        m_AlreadyHittingBoat = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckForPlayer(collision);
        if (!m_AlreadyHittingBoat)
        {
            StartCoroutine(CheckForBoat(collision));

        }
    }
    private IEnumerator CheckForBoat(Collision collision)
    {
        PlayerShip playerShip = collision.gameObject.GetComponentInParent<PlayerShip>();
        if (playerShip != null)
        {
            m_AlreadyHittingBoat = true;
            playerShip.TakeDamage(50);
            yield return new WaitForSeconds(1);
            m_AlreadyHittingBoat = false;
        }
    }

    public Vector3 GetLastLocation()
    {
        return m_PlayersLastPos;
    }

    private void CheckForPlayer(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            m_HasBeenOnIsland = true;
            m_PlayersLastPos = player.transform.position;
        }
    }
}
