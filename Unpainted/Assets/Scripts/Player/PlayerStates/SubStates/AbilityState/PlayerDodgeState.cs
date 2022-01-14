using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerAbilityState
{
    //Only be able to dodge in certain angles angles https://youtu.be/7YnmIE6R7Y4?t=2358
    public bool CanDodge { get; set; }

    public float lastDodgeTime;

    private Vector2 dodgeDirection;
    private Vector2 dodgeDirectionInput;

    public PlayerDodgeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        dodgeDirection = player.HoldDodgeState.dodgeDirection;

        m_StartTime = Time.time;
        player.rb.drag = playerData.dodgeDrag;
        player.SetVelocity(playerData.dodgeVelocity, dodgeDirection);
    }

    public override void Exit()
    {
        base.Exit();

        if (player.CurrentVelocity.y > 0)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.dodgeEndYMultiplier);
        }
    }

    public override void LogicUppdate()
    {
        base.LogicUppdate();

        if (!isExitingState)
        {
            if (Time.time >= m_StartTime + playerData.dodgeTime)
            {
                player.rb.drag = 0f;
                isAbilityDone = true;
                lastDodgeTime = Time.time;
            }
        }
    }


    public bool CheckIfCanDodge()
    {
        return CanDodge && Time.time >= lastDodgeTime + playerData.dodgeCooldown;
    }

    public void ResetCanDodge()
    {
        CanDodge = true;
    }

    private void StartDodgeMovement()
    {

    }
}
