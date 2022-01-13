using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerAbilityState
{
    public bool canDoubleJump;
    public PlayerDoubleJumpState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();


        player.SetVelocityY(playerData.doubleJumpVelocity);
        player.InAirState.SetIsDoubleJumping();
        canDoubleJump = false;
        DoChecks();
        isAbilityDone = true;
    }

    public void ResetDoubleJump() => canDoubleJump = true;
}
