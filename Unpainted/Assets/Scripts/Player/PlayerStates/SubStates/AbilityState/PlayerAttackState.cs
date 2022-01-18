using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    public bool dodgeAfter;
    private bool dodgeInput;
    private float xInput;
    public float lastAttackTime;

   
    private Vector2 attackDirection;
    private Vector2 attackDirectionInput;

    private List<IDamagable> detecteDamagables = new List<IDamagable>();
    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }


    #region Enter/Exit/Do checks
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

    #endregion

    #region Uppdates
    public override void LogicUppdate()
    {
        base.LogicUppdate();
        dodgeInput = player.InputHandler.DodgeInput;
        xInput = player.InputHandler.NormalizedInputX;

        if (dodgeAfter && AnimationAllowChangeState)
        {
            dodgeAfter = false;
            stateMachine.ChangeState(player.DodgeState);
        }
        else if (dodgeInput && AnimationAllowChangeState)
        {
            stateMachine.ChangeState(player.HoldDodgeState);
        }
        else if (isAnimationFinished)
        {
            isAbilityDone = true;
        }
        else
        {
            core.Movement.SetVelocityX(xInput * playerData.movementVelocity);
        }
    }

    public override void PhysicsUppdate()
    {
        base.PhysicsUppdate();
    }

    #endregion

    #region AttackEvents

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

    }

    public void Attack()
    {

    }
    public void AddToDetectedList(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<IDamagable>();

        if (damagable != null)
        {
            detecteDamagables.Add(damagable);
        }
    }

    public void RemoveFromDetectedList(Collider2D collision)
    {
        IDamagable damagable = collision.GetComponent<IDamagable>();

        if (damagable != null)
        {
            detecteDamagables.Remove(damagable);
        }
    }
    #endregion

    #region Cooldown Check
    public bool CheckIfCanAttack()
    {
        return Time.time >= lastAttackTime + playerData.attackCooldown;
    }

    #endregion
}
