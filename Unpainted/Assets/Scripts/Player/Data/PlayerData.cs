using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]

public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 20;

    [Header("Jump State")]
    public float jumpVelocity = 30;
    public float doubleJumpVelocity = 20;
    public float shortJumpMulitplier = 0.5f;

    [Header("InAir State")]
    public float hangTime = 0.15f;
    public float gravityScale = 5;
    public float gravityScaleFallingMultiplier = 1.7f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 4;

    [Header("Dash State")]
    public float dashCooldown = 0.5f;
    public float dashTime = 0.2f;
    public float dashDrag = 10;
    public float dashVelocity = 60;
    public float dashEndYMultiplier = 0.2f;

    [Header("Dodge State")]
    public float dodgeCooldown = 1f;
    public float dodgeTime = 0.3f;
    public float dodgeMoveTime = 0.1f;
    public float dodgeVelocity = 70;
    public float dodgeVelocityMultiplier = 0.95f;
    public float dodgeDrag = 10;
    public float dodgeEndYMultiplier = 0.3f;
    public float dodgeMaxHoldTime = 2;
    public float DodgeMovementSmoothing = 15f;

    public float dodgeHoldTimeScale = 0.5f;
    public float dodgeTimeScaleStartTimer = 0.1f;


    [Header("Attack State")]
    public float damage = 5;
    public float attackCooldown = 0.2f;
    public float attackDistanceFromPlayer = 4;
    public float attackCircleSize = 10;
    public LayerMask damageable;


}
