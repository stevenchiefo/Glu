using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public GameObject m_Selected;

    private void OnTriggerEnter(Collider info)
    {
        m_Selected = info.gameObject;
    }
}