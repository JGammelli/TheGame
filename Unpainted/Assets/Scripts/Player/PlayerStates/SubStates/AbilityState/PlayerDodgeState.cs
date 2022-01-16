using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerAbilityState
{
    //Only be able to dodge in certain angles angles https://youtu.be/7YnmIE6R7Y4?t=2358
    public bool CanDodge { get; set; }

    public float lastDodgeTime;

    private int xInput;
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
        core.Movement.SetVelocity(playerData.dodgeVelocity, dodgeDirection);
    }

    public override void Exit()
    {
        base.Exit();
        CanDodge = false;



        if (core.Movement.CurrentVelocity.y > 0)
        {
            core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * playerData.dodgeEndYMultiplier);
            Debug.Log("dodge end multiplier");
        }
    }

    public override void LogicUppdate()
    {
        base.LogicUppdate();
        xInput = xInput = player.InputHandler.NormalizedInputX;

        if (!isExitingState)
        {
            if (Time.time >= m_StartTime + playerData.dodgeTime + playerData.dodgeMoveTime)
            {
                player.rb.drag = 0;
                isAbilityDone = true;
                lastDodgeTime = Time.time;
            }
            else if (Time.time >= m_StartTime + playerData.dodgeTime)
            {

                core.Movement.SetVelocitySmooth(playerData.DodgeMovementSmoothing * xInput, (core.Movement.CurrentVelocity.y * 0.2f));
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
