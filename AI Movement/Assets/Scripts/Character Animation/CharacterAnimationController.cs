using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator m_Animator;

    private float m_Speed;
    private float m_IdleTime;

    public float Speed
    {
        get
        {
            return m_Speed;
        }
        set
        {
            if (value != m_Speed)
            {
                m_Speed = value;
                m_IdleTime = 0;
                SetIdleAnimation();
                SetSpeedAnimation();
                return;
            }
            else
            {
                m_IdleTime += Time.deltaTime;
                SetIdleAnimation();
                m_Speed = value;
            }
        }
    }



    void Start()
    {
        LoadAnimator();
    }

    private void SetIdleAnimation()
    {
        m_Animator.SetBool("ArmsCrossed", m_IdleTime >= 3);
    }

    private void SetSpeedAnimation()
    {
        m_Animator.SetFloat("Speed", m_Speed);
    }

    private void LoadAnimator()
    {
        m_Animator = GetComponent<Animator>();
    }
}
