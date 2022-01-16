using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }

    private float lastDashTime;
    private Vector2 dashDirection;
    private bool attackAtEnd;

    private bool attackInput;
    private bool dodgeInput;

    public PlayerDashState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        CanDash = false;
        player.rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        player.InputHandler.UseDashInput();
        dashDirection = Vector2.right * core.Movement.FacingDirection;
        m_StartTime = Time.time;
        attackAtEnd = false;
        particleHandler.PlayEffect(particleHandler.dashOnGroundEffect, particleHandler.dashOnGroundEffect.transform.position, particleHandler.dashOnGroundEffect.transform.rotation.eulerAngles);
    }

    public override void Exit()
    {
        base.Exit();
        particleHandler.StopEffect(particleHandler.dashOnGroundEffect);
    }

    public override void LogicUppdate()
    {
        base.LogicUppdate();

        dodgeInput = player.InputHandler.DodgeInput;
        attackInput = player.InputHandler.AttackInput;

        if (!isExitingState)
        {
            player.rb.drag = playerData.dashDrag;
            core.Movement.SetVelocity(playerData.dashVelocity, dashDirection);
            
        }

        if (attackInput)
        {
            attackAtEnd = true;
        }

        if (dodgeInput && player.DodgeState.CheckIfCanDodge())
        {
            EndDashResets();
            stateMachine.ChangeState(player.HoldDodgeState);
        }
        else if (Time.time >= m_StartTime + playerData.dashTime)
        {
            EndDashResets();
            if (attackAtEnd)
            {
                stateMachine.ChangeState(player.AttackState);
            }
            else isAbilityDone = true;
        }
    }


    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void ResetCanDash()
    {
        CanDash = true;
    }

    private void EndDashResets()
    {
        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        lastDashTime = Time.time;
        player.rb.drag = 0f;
        core.Movement.SetVelocityX(core.Movement.CurrentVelocity.x * 0.2f);
    }

}
