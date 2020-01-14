using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator m_Anitmation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_Anitmation.Play("TreeFallingDown");
    }
}