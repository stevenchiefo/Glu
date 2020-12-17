using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : PowerUp
{
    protected override void OnHit(Player player)
    {
        player.SetPowerUp(PowerUps.Bounce, Duration);
    }
}