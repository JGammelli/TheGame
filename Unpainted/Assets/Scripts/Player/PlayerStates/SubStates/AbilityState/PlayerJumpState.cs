using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{

    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();


        player.SetVelocityY(playerData.jumpVelocity);
        player.InAirState.SetIsJumping();
        DoChecks();
        isAbilityDone = true;
    }
}
