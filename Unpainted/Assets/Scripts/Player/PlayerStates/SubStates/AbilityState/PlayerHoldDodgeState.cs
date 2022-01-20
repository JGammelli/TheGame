using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldDodgeState : PlayerAbilityState
{

    public bool dodgeInputStop;

    public Vector2 dodgeDirection;
    private Vector2 dodgeDirectionInput;
    public PlayerHoldDodgeState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, PlayerParticleHandler particleHandler, string m_AnimatorBoolName) : base(player, playerStateMachine, playerData, particleHandler, m_AnimatorBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseDodgeInput();
        dodgeDirection = Vector2.right * -core.Movement.FacingDirection;
        m_StartTime = Time.unscaledTime;
        Debug.Log("hold dodge enter");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUppdate()
    {
        base.LogicUppdate();

        if (!isExitingState)
        {
            dodgeDirectionInput = player.InputHandler.DodgeDirectionInput;
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

            core.Movement.CheckIfShouldFlipMousePos(-dodgeDirection);
            float angle = Vector2.SignedAngle(Vector2.right, dodgeDirection);
            player.DodgeDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 180);

            if (dodgeInputStop || Time.unscaledTime >= m_StartTime + playerData.dodgeMaxHoldTime)
            {
                Time.timeScale = 1;
                player.DodgeDirectionIndicator.gameObject.SetActive(false);
                player.AttackState.dodgeAfter = true;
                stateMachine.ChangeState(player.AttackState);
                Debug.Log("hold dodge exit");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
