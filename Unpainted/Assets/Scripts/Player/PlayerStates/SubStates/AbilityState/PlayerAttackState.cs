using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    public bool dodgeAfter;
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
        attackDirection = Vector2.right * player.facingDirection;

        if (attackDirectionInput != Vector2.zero)
        {
            attackDirection = attackDirectionInput;
            attackDirection.Normalize();
        }

        
        Vector2 attackPoint = player.transform.position;

        float angle = Vector2.SignedAngle(Vector2.right, attackDirection);
        particleHandler.PlayEffect(particleHandler.slashEffect, attackPoint, new Vector3(0f, 0f, angle));
        Debug.Log("Attack towards " + attackDirection);

        lastAttackTime = Time.time;


    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUppdate()
    {
        base.LogicUppdate();
        if (dodgeAfter)
        {
            dodgeAfter = false;
            stateMachine.ChangeState(player.DodgeState);
        }
        else isAbilityDone = true;
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
