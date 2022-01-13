using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Components

    private PlayerInput playerInput;
    [SerializeField]
    private Camera cam;

    #endregion

    #region Inputs
    public Vector2 RawMovementInput { get; private set; }
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool DashInput { get; private set; }
    public bool DodgeInput { get; private set; }
    public bool DodgeInputStop { get; private set; }
    public bool AttackInput { get; private set; }
    public Vector2 RawDodgeDirectionInput { get; private set; }
    public Vector2 RawAttackDirectionInput { get; private set; }
    public Vector2 MousePositionInput { get; private set; }

    #endregion

    #region Timers

    [SerializeField]
    private float inputBufferTime;

    private float jumpInputTimer;
    private float dodgeInputTimer;
    private float dashInputTimer;


    #endregion

    #region Unity CallBacks

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
    }
    private void Update()
    {
        CheckInputBuffer();
    }

    #endregion

    #region Move Input
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormalizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormalizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }

    #endregion

    #region Jump Input
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInputStop = false;
            JumpInput = true;
            jumpInputTimer = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;   
        }
    }
    public void UseJumpInput() => JumpInput = false;

    #endregion

    #region Attack Input

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
        }
    }

    public void UseAttackInput() => AttackInput = false;

    #endregion

    #region Dash Input

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
        }
    }

    public void UseDashInput() => DashInput = false;

    #endregion

    #region Dodge Input

    public void OnDodgeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DodgeInputStop = false;
            DodgeInput = true;
            dodgeInputTimer = Time.time;
        }
        else if (context.canceled)
        {
            DodgeInputStop = true;
        }
    }

    public void UseDodgeInput() => DodgeInput = false;

    public void OnMouseDirectionInput(InputAction.CallbackContext context)
    {
        RawDodgeDirectionInput = -context.ReadValue<Vector2>();
        RawAttackDirectionInput = context.ReadValue<Vector2>();

        if (playerInput.currentControlScheme == "Keyboard")
        {
            MousePositionInput = cam.ScreenToWorldPoint(new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 1f));

            RawDodgeDirectionInput = -((Vector3)MousePositionInput - transform.position);
            RawAttackDirectionInput = (Vector3)MousePositionInput - transform.position;
        }
    }

    #endregion

    #region BufferChecks
    private void CheckInputBuffer()
    {
        if (Time.time >= jumpInputTimer + inputBufferTime)
        {
            JumpInput = false;
        }
        if (Time.time >= dodgeInputTimer + inputBufferTime)
        {
            DodgeInput = false;
        }
    }

    #endregion
}
