using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    [SerializeField] private float m_DashSpeed = 5f;
    private float[] m_CoolDownTimers = new float[3];
    [SerializeField] private float[] m_CoolDownTime = new float[3];

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Updater();
        CooldownCheckers();
    }

    protected override void Ability1()
    {
        if (m_Ability1 == false)
        {
            m_DirectionOfWalk = m_DirectionOfWalk.normalized * m_DashSpeed * 2f * Time.deltaTime;
            m_Ability1 = true;
        }
    }

    public override void OnReset()
    {
        if (m_Ability1 == false)
            base.OnReset();
    }

    public override void OnMove(UnityEngine.InputSystem.InputValue value)
    {
        if (m_Ability1 == false)
            base.OnMove(value);
    }

    private void CooldownCheckers()
    {
        if (m_Ability1 == true)
        {
            m_CoolDownTimers[0] += Time.deltaTime;
            float cooldowntime1 = 1f / m_CoolDownTime[0];
            if (m_CoolDownTimers[0] >= cooldowntime1)
            {
                m_CoolDownTimers[0] = 0f;
                m_DirectionOfWalk = Vector2.zero;
                m_Ability1 = false;
            }
        }
        if (m_Ability2 == true)
        {
            m_CoolDownTimers[1] += Time.deltaTime;
            float cooldowntime2 = 1f / m_CoolDownTime[1];
            if (m_CoolDownTimers[1] >= cooldowntime2)
            {
                m_CoolDownTimers[1] = 0f;
                m_Ability2 = false;
            }
        }
        if (m_Ability3 == true)
        {
            m_CoolDownTimers[2] += Time.deltaTime;
            float cooldowntime3 = 1f / m_CoolDownTime[2];
            if (m_CoolDownTimers[2] >= cooldowntime3)
            {
                m_CoolDownTimers[2] = 0f;
                m_Ability3 = false;
            }
        }
    }
}