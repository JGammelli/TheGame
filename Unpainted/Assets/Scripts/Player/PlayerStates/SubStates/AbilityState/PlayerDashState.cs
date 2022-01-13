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
        player.InputHandler.UseDashInput();
        dashDirection = Vector2.right * player.facingDirection;
        m_StartTime = Time.time;
        attackAtEnd = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUppdate()
    {
        base.LogicUppdate();

        dodgeInput = player.InputHandler.DodgeInput;
        attackInput = player.InputHandler.AttackInput;

        if (!isExitingState)
        {
            player.rb.drag = playerData.dashDrag;
            player.SetVelocity(playerData.dashVelocity, dashDirection);
        }

        if (attackInput)
        {
            attackAtEnd = true;
        }

        if (dodgeInput && player.DodgeState.CheckIfCanDodge())
        {
            lastDashTime = Time.time;
            player.rb.drag = 0f;

            if (attackAtEnd)
            {
                stateMachine.ChangeState(player.AttackDodgeState);
                Debug.Log("AttackDodgeState");
            }
            else stateMachine.ChangeState(player.DodgeState);
        }
        else if (Time.time >= m_StartTime + playerData.dashTime)
        {
            player.rb.drag = 0f;
            isAbilityDone = true;
            lastDashTime = Time.time;
            if (attackAtEnd)
            {
                stateMachine.ChangeState(player.AttackState);
                Debug.Log("Dash with attack at end");
            }
            // if Attackinput pressed during dash and dash cancled by dodge, attack before dodging
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

}
