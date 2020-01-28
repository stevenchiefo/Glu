using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public enum ElementState
    {
        Fire = 0,
        Ice,
    }

    public ElementState Elementstate = ElementState.Fire;
    public Vector2 MovementDirection;
    public bool Attack = false;
    public float LookDirection = 0f;

    private void OnAttack()
    {
        if (Attack == false)
        {
            Attack = true;
        }
    }

    private void OnSwitch()
    {
    }

    private void OnMove(InputValue value)
    {
        MovementDirection = new Vector2(value.Get<Vector2>().x, 0f);
    }

    private void OnMoveReset()
    {
        MovementDirection = Vector2.zero;
    }

    private void OnLook(InputValue value)
    {
        LookDirection = value.Get<Vector2>().x;
    }

    private void OnLookReset()
    {
        LookDirection = 0f;
    }

    public void ResetAttack() => Attack = false;
}