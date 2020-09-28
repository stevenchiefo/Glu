using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator m_UpperBody;
    [SerializeField] private Animator m_LowerBody;
    private PlayerMovement m_PlayerMovement;

    private void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForRunning();
    }

    private void CheckForRunning()
    {
        string RunPar = "IsRunning";
        m_UpperBody.SetBool(RunPar, m_PlayerMovement.IsRunning);
        m_LowerBody.SetBool(RunPar, m_PlayerMovement.IsRunning);
    }
}