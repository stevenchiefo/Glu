using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DockShipTimer());
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private IEnumerator DockShipTimer()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerInterfaceUI.Instance.UpdateUI();
        yield return new WaitForSeconds(0.1f);
        DockingManager.Instance.RespawnBoat(PlayerShip.Instance);
    }
}