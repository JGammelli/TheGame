using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    public bool dodgeAfter;
    private bool dodgeInput;
    public float lastAttackTime;

    public float attackDistance;
    public float attackRadius;
    private Vector2 attackDirection;
    private Vector2 attackDirectionInput;
    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseAttackInput();
        attackDirectionInput = player.InputHandler.AttackDirectionInput;
        attackDirection = Vector2.right * core.Movement.FacingDirection;

        if (attackDirectionInput != Vector2.zero)
        {
            attackDirection = attackDirectionInput;
            attackDirection.Normalize();
        }

        player.Animator.SetFloat("mousePositionY", attackDirection.y);
        Debug.Log(attackDirection.y);


        core.Movement.CheckIfShouldFlipMousePos(attackDirection);

        lastAttackTime = Time.time;


    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUppdate()
    {
        base.LogicUppdate();
        dodgeInput = player.InputHandler.DodgeInput;

        if (dodgeAfter)
        {
            dodgeAfter = false;
            stateMachine.ChangeState(player.DodgeState);
        } else if (dodgeInput)
        {
            stateMachine.ChangeState(player.HoldDodgeState);
        }
        else if (isAnimationFinished)
        {
            isAbilityDone = true;
        }
    }

    public override void PhysicsUppdate()
    {
        base.PhysicsUppdate();
    }

    public bool CheckIfCanAttack()
    {
        return Time.time >= lastAttackTime + playerData.attackCooldown;
    }
}
