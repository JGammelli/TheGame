using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
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
        attackDirectionInput = player.InputHandler.RawAttackDirectionInput;
        attackDirection = Vector2.right * player.facingDirection;

        if (attackDirectionInput != Vector2.zero)
        {
            attackDirection = attackDirectionInput;
            attackDirection.Normalize();
        }

        
        Vector2 attackPoint = player.transform.position;

        /*
        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPoint, playerData.attackCircleSize, playerData.damageable);

        Debug.Log(attackDirectionInput);
+
        for (int i = 0; i < targets.Length; i++)
        {
            Debug.Log(targets[i]);
        }*/


        particleHandler.PlayEffect(particleHandler.slashEffect, attackPoint);
        Debug.Log("attack");
        Debug.Log(particleHandler.slashEffect.transform.position);
        Debug.Log(player.transform.position);

        isAbilityDone = true;
        lastAttackTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUppdate()
    {
        base.LogicUppdate();

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
