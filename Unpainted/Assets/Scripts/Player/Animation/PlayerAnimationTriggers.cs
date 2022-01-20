using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    public Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void PlayerAnimationTrigger() => player.StateMachine.CurrentState.AnimationTrigger();
    private void PlayerAnimationAllowChangeState() => player.StateMachine.CurrentState.AnimationCanChangeStateTrigger();
    private void PlayerAnimationFinishedTrigger()
    {
        player.StateMachine.CurrentState.AnimationFinishedTrigger();
    }
}
