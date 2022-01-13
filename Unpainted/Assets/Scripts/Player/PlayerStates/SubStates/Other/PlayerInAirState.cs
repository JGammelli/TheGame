using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{

    //Inputs
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool dashInput;
    private bool dodgeInput;
    private bool attackInput;

    //Checks
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isJumping;
    private bool isDoubleJumping;

    private bool hangTime;

    public PlayerInAirState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUppdate()
    {
        base.LogicUppdate();

        CheckHangTime();
        CheckInput();
        CheckJumpMultiplier();
        player.rb.gravityScale = playerData.gravityScale;


        if (isGrounded)
        {
            stateMachine.ChangeState(player.LandState);
            hangTime = false;
        }
        else if (jumpInput && hangTime)
        {
            player.InputHandler.UseJumpInput();
            hangTime = false;
            stateMachine.ChangeState(player.JumpState);
        }
        else if (jumpInput && player.DoubleJumpState.canDoubleJump)
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.DoubleJumpState);
        }
        else if (isTouchingWall && xInput == player.facingDirection && player.CurrentVelocity.y < -10)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (dodgeInput && player.DodgeState.CheckIfCanDodge())
        {
            stateMachine.ChangeState(player.DodgeState);
        }
        else if (attackInput && player.AttackState.CheckIfCanAttack())
        {
            stateMachine.ChangeState(player.AttackState);
        }
        else
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);
            if (player.CurrentVelocity.y < -2)
            {
                player.rb.gravityScale *= playerData.gravityScaleFallingMultiplier;
            }
        }

    }

    public override void PhysicsUppdate()
    {
        base.PhysicsUppdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();

    }

    private void CheckHangTime()
    {
        if (hangTime && Time.time > playerData.hangTime + m_StartTime)
        {
            hangTime = false;
        }
    }

    private void StartHangTime() => hangTime = true;

    private void CheckInput()
    {
        xInput = player.InputHandler.NormalizedInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        dashInput = player.InputHandler.DashInput;
        dodgeInput = player.InputHandler.DodgeInput;
        attackInput = player.InputHandler.AttackInput;
    }
    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.shortJumpMulitplier);
                isJumping = false;
            }
            
            if (player.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
                isDoubleJumping = false;
            }
        }
    }

    public void SetIsJumping() => isJumping = true;
    public void SetIsDoubleJumping() => isDoubleJumping = true;
    public void GroundedToInAir()
    {
        StartHangTime();
    }
}
