using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    //StateMachine
    public PlayerStateMachine StateMachine { get; private set; }
    [SerializeField] 
    private PlayerData playerData;



//States
public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDoubleJumpState DoubleJumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerHoldDodgeState HoldDodgeState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }


    #endregion

    #region Components
    //Player Components
    public Animator Animator { get; private set; }
    public Rigidbody2D rb;
    public Collider2D collider2d;
    public PlayerInputHandler InputHandler { get; private set; }
    public Transform DodgeDirectionIndicator { get; private set; }
    public PlayerParticleHandler ParticleHandler { get; private set; }

    #endregion

    #region Check Variables

    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;


    #endregion

    #region Other Variables 
    //Publics
    public Vector2 CurrentVelocity { get; private set; }
    public int facingDirection { get; private set; }

    //Privates
    private Vector2 workspace;

    #endregion


    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        ParticleHandler = GetComponent<PlayerParticleHandler>();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, ParticleHandler, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, ParticleHandler, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, ParticleHandler, "inAir");
        DoubleJumpState = new PlayerDoubleJumpState(this, StateMachine, playerData, ParticleHandler, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, ParticleHandler, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, ParticleHandler, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, ParticleHandler, "wallSlide");
        DashState = new PlayerDashState(this, StateMachine, playerData, ParticleHandler, "dash");
        HoldDodgeState = new PlayerHoldDodgeState(this, StateMachine, playerData, ParticleHandler, "holdDodge");
        DodgeState = new PlayerDodgeState(this, StateMachine, playerData, ParticleHandler, "inAir");
        AttackState = new PlayerAttackState(this, StateMachine, playerData, ParticleHandler, "attack");


    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        DodgeDirectionIndicator = transform.Find("DodgeDirectionIndicator");

        facingDirection = 1;

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = rb.velocity;
        StateMachine.CurrentState.LogicUppdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUppdate();
    }

    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    #endregion

    #region Check Functions
    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != facingDirection)
        {
            Flip();
        }
    }

    public bool CheckIfGrounded()
    {
        Vector3 centerLowY = new Vector3(collider2d.bounds.center.x, (collider2d.bounds.center.y - (collider2d.bounds.size.y / 2)), 0);
        Vector3 sizeLowY = new Vector3(collider2d.bounds.size.x - 0.2f, 0.3f, 0);

        if (CurrentVelocity.y <= 3f)
        {
            return Physics2D.OverlapBox(centerLowY, sizeLowY, 0.0f, whatIsGround);
        }
        else return false;
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(collider2d.bounds.center, transform.right, (collider2d.bounds.size.x / 2 + 0.2f), whatIsGround);
    }

    #endregion

    #region Other Functions //Flip etc

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishedTrigger();
    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        //    Vector3 centerLowY = new Vector3(collider2d.bounds.center.x, (collider2d.bounds.center.y - (collider2d.bounds.size.y / 2)), 0);
        //    Vector3 sizeLowY = new Vector3(collider2d.bounds.size.x - 0.2f, 0.1f, 0);

        //    Gizmos.DrawWireCube(centerLowY, sizeLowY)
    }

    #endregion
}
