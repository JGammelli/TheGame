using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    //Inputs
    protected int xInput;
    private bool jumpInput;
    private bool dashInput;
    private bool dodgeInput;
    private bool attackInput;

    //Checks
    protected bool isGrounded;
    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = core.CollisionSenses.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        player.DoubleJumpState.ResetDoubleJump();
        player.DashState.ResetCanDash();
        player.DodgeState.ResetCanDodge();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUppdate()
    {


        base.LogicUppdate();

        CheckInput();


        if (jumpInput)
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        } 
        else if (!isGrounded)
        {
            stateMachine.ChangeState(player.InAirState);
            player.InAirState.GroundedToInAir();
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (dodgeInput && player.DodgeState.CheckIfCanDodge())
        {
            stateMachine.ChangeState(player.HoldDodgeState);
        }
        else if (attackInput && player.AttackState.CheckIfCanAttack())
        {
            stateMachine.ChangeState(player.AttackState);
        }
    }

    public override void PhysicsUppdate()
    {
        base.PhysicsUppdate();
    }

    protected void CheckInput()
    {

        xInput = player.InputHandler.NormalizedInputX;
        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;
        dodgeInput = player.InputHandler.DodgeInput;
        attackInput = player.InputHandler.AttackInput;
    }
}
