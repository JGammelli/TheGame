using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core core;
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected PlayerParticleHandler particleHandler;

    protected float m_StartTime;
    protected bool isExitingState;

    protected bool isAnimationFinished;
    protected bool AnimationAllowChangeState;
    private string animatorBoolString;


    public PlayerState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName)
    {
        this.player = player;
        this.stateMachine = playerStateMachine;
        this.playerData = playerData;
        this.particleHandler = particleHandler;
        this.animatorBoolString = m_AnimatorBoolName;
        core = player.Core;
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Animator.SetBool(animatorBoolString, true);
        m_StartTime = Time.time;
        isAnimationFinished = false;
        AnimationAllowChangeState = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        isExitingState = true;
        player.Animator.SetBool(animatorBoolString, false);
    }


    public virtual void LogicUppdate() { }


    public virtual void PhysicsUppdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationActionTrigger() { }

    public virtual void AnimationCanChangeStateTrigger() => AnimationAllowChangeState = true;

    public virtual void AnimationFinishedTrigger() => isAnimationFinished = true;

}
