using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerAbilityState
{
    //Only be able to dodge in certain angles angles https://youtu.be/7YnmIE6R7Y4?t=2358
    public bool CanDodge { get; set; }

    public float lastDodgeTime;
    public bool isHolding;
    public bool dodgeInputStop;

    private Vector2 dodgeDirection;
    private Vector2 dodgeDirectionInput;
    public PlayerDodgeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        CanDodge = false;
        player.InputHandler.UseDodgeInput();
        isHolding = true;
        dodgeDirection = Vector2.right * -player.facingDirection;
        m_StartTime = Time.unscaledTime;
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
            if (isHolding)
            {
                dodgeDirectionInput = player.InputHandler.RawDodgeDirectionInput;
                dodgeInputStop = player.InputHandler.DodgeInputStop;

                if (Time.unscaledTime >= m_StartTime + playerData.dodgeTimeScaleStartTimer)
                {
                    Time.timeScale = playerData.dodgeHoldTimeScale;
                    player.DodgeDirectionIndicator.gameObject.SetActive(true);
                }

                if (dodgeDirectionInput != Vector2.zero)
                {
                    dodgeDirection = dodgeDirectionInput;
                    dodgeDirection.Normalize();
                }

                float angle = Vector2.SignedAngle(Vector2.right, dodgeDirection);
                player.DodgeDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 180);

                if (dodgeInputStop || Time.unscaledTime >= m_StartTime + playerData.dodgeMaxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1;
                    m_StartTime = Time.time;
                    player.rb.drag = playerData.dodgeDrag;
                    player.SetVelocity(playerData.dodgeVelocity, dodgeDirection);
                    player.DodgeDirectionIndicator.gameObject.SetActive(false);
                }
            }
            else
            {
                if (Time.time >= m_StartTime + playerData.dodgeTime)
                {
                    player.rb.drag = 0f;
                    isAbilityDone = true;
                    lastDodgeTime = Time.time;
                }
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
}
