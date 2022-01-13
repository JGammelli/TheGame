using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Choose in Inpector")]
    [Space()]
    [Space()]
    [Header("Movement")]
    public float m_MovementSpeed;

    [Header("Jumping and Air Movement")]
    public float m_JumpForce;
    public float m_ShortJumpMuliplier;
    public float m_AirDragMultiplier = 0.95f;
    public float m_InputBufferJump;
    [Header("Double Jump")]
    public bool m_CanDoubleJump;
    public float m_DoubleJumpForce;

    [Header("WallSlide")]
    public bool m_CanWallSlide;
    public float m_WallSlideSpeed;

    [Header("GroundCheck")]
    public LayerMask m_WhatIsGround;

    private float m_MovementInputDirection;
    private bool m_DoubleJumpLeft;
    private float m_JumpInputTimer = 0.1f;
    private int m_FacingDirection = 1;

    private bool m_IsFacingRight = true;
    private bool m_IsWalking;
    private bool m_IsGrounded;
    private bool m_IsWallSliding;
    private bool m_IsTouchingWall;
    private bool m_IsAttemptingToJump;
    private bool m_CanJump;


    Collider2D m_Collider2D;
    Animator m_Animator;
    Rigidbody2D m_Rigidbody;


    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();

        if (m_CanDoubleJump)
        {
            m_DoubleJumpLeft = true;
        }
    }

    void Update()
    {
        CheckInput();
        HandleInputBuffer();
        CheckMovementDirection();
        GroundCheck();
        WallCheck();
        CheckIfWallSliding(m_CanWallSlide);
        CheckJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckInput()
    {
        m_MovementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        { /*
            if (m_IsGrounded || (m_JumpsLeft > 0))
            {
                Jump();
            }
            else
            {

            } */
            m_JumpInputTimer = m_InputBufferJump;
            m_IsAttemptingToJump = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y * m_ShortJumpMuliplier);
        }
    }

    private void HandleInputBuffer()
    {
        if (m_JumpInputTimer > -1)
        { 
            m_JumpInputTimer -= Time.deltaTime;
        }
    }
    private void CheckMovementDirection()
    {
        if (m_IsFacingRight && m_MovementInputDirection < 0)
        {
            Flip();
        }
        if (!m_IsFacingRight && m_MovementInputDirection > 0)
        {
            Flip();
        }
    }

    private void GroundCheck()
    {
        m_IsGrounded = false;
        Collider2D[] groundCheck = GetGroundCheck();
        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (groundCheck[i].gameObject != gameObject)
            {
                m_IsGrounded = true;
            }
        }
    }

    private Collider2D[] GetGroundCheck()
    {
        Vector3 centerLowY = new Vector3(m_Collider2D.bounds.center.x, (m_Collider2D.bounds.center.y - (m_Collider2D.bounds.size.y / 2)), 0);
        Vector3 sizeLowY = new Vector3(m_Collider2D.bounds.size.x - 0.2f, 0.1f, 0);
        return Physics2D.OverlapBoxAll(centerLowY, sizeLowY, 0.0f, m_WhatIsGround);
    }

    private void WallCheck()
    {
        m_IsTouchingWall = Physics2D.Raycast(m_Collider2D.bounds.center, transform.right, (m_Collider2D.bounds.size.x / 2 + 0.2f), m_WhatIsGround); 
    }

    private void CheckIfWallSliding(bool canWallSlide)
    {
        if (canWallSlide)
        {
            if (!m_IsGrounded && m_IsTouchingWall && m_Rigidbody.velocity.y < 0 && m_MovementInputDirection == m_FacingDirection)
            {
                m_IsWallSliding = true;
            }
            else m_IsWallSliding = false;
        }
    }

    private void CheckJump()
    {
        if (m_IsGrounded && m_Rigidbody.velocity.y < 1 && m_CanDoubleJump)
        {
            m_DoubleJumpLeft = true;
        }

        if (m_DoubleJumpLeft || m_IsGrounded)
        {
            m_CanJump = true;
        }
        else m_CanJump = false;



        if (m_JumpInputTimer > 0)
        {
            m_IsAttemptingToJump = true;
        }
        else m_IsAttemptingToJump = false;

        if (m_IsAttemptingToJump)
        {
            if (m_IsGrounded)
            {
                NormalJump();
            }
            else if (m_CanDoubleJump && m_DoubleJumpLeft)
            {
                DoubleJump();
            }
        }
    }

    private void NormalJump()
    {
        m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpForce);
        m_IsAttemptingToJump = false;
        m_JumpInputTimer = -1;
    }

    private void DoubleJump()
    {
        m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_DoubleJumpForce);
        m_IsAttemptingToJump = false;
        m_JumpInputTimer = -1;
        m_DoubleJumpLeft = false;
    }

    private void ApplyMovement()
    {


        if (!m_IsGrounded && !m_IsWallSliding && m_MovementInputDirection == 0)
        {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x * m_AirDragMultiplier, m_Rigidbody.velocity.y);
        }
        else
        {
            m_Rigidbody.velocity = new Vector2(m_MovementSpeed * m_MovementInputDirection, m_Rigidbody.velocity.y);
        }


        if (m_IsWallSliding)
        {
            if (m_Rigidbody.velocity.y < -m_WallSlideSpeed)
            {
                m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, -m_WallSlideSpeed);
            }
        }
    }



    private void Flip()
    {
        m_FacingDirection *= -1;
        m_IsFacingRight = !m_IsFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {

    }
}
