using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimationControllor : MonoBehaviour
{
    public void RespawnPlayerShip()
    {
        PlayerShip.Instance.DestroyShip();
    }
}