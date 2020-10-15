using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            ball.ResetBall();
            BlockSpawner.Instance.ResetAllBlocks();
            ScoreManager.Instance.ResetScore();
        }
    }
}