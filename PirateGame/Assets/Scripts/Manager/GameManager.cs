using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(DockShipTimer());
    }


    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private IEnumerator DockShipTimer()
    {
        yield return new WaitForSeconds(1);
        DockingManager.Instance.RespawnBoat(PlayerShip.Instance,Player.Instance);
    }
    
}
