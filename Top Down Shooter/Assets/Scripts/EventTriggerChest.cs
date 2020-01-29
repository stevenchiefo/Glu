using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerChest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<GameManager>().OpenDoor();
            gameObject.SetActive(false);
        }
    }
}