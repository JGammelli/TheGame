using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    #region Constants

    #region Movement
    [Header("Movement")]
    [SerializeField] private float m_MovementSpeed = 40.0f;
    [Range(0, .7f)] [SerializeField] private float m_MovementSmoothing = .05f;
    #endregion

    [Space()]
    [Header("Jumping")]
    [SerializeField] private float m_Jumpforce = 400.0f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private bool m_HalfJumps = false;
    [Range(0, .3f)] [SerializeField] private float m_HangTime = .1f;
    [SerializeField] private float m_FallMultiplier = 2.5f;
    [SerializeField] private float m_FallMulitplierStartVelocity = 0;
    [SerializeField] private float m_InputBufferJump = 0.1f;

    [Space()]
    [Header("Ground Check")]
    [SerializeField] private LayerMask m_WhatIsGround;

    [Space()]
    [Header("Dash")]
    [SerializeField] private bool m_DashActive = false;
    [SerializeField] private float m_DashSpeed = 100;
    [SerializeField] private float m_DashCooldown = 0.5f;
    [SerializeField] private float m_StartDashTime = 0.1f;


    #endregion

    private bool m_Grounded;
    private float m_HorizontalMovement;
    private bool m_Jump = false;
    private bool m_SmallJump = false;
    private float m_InputBufferJumpTimer;
    private float m_HangCounter = 0f;

    private bool m_Dash = false;
    private float m_DashTime;
    private float m_DashCooldownTimer;
    private int m_DashDirection;


    private Rigidbody2D m_Rigidbody2D;
    private Collider2D m_Collider2D;
    private Vector3 m_Velocity = Vector3.zero;
    private bool m_FacingRight = true;
    private bool m_FlipDisabled = false;

    private void Awake()
    {
        m_DashTime = m_StartDashTime;
        m_Collider2D = GetComponent<Collider2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        HorizontalInput();
        JumpInput();
        HalfJumpInput();
        DashInput();



        //Movement ray
        Debug.DrawRay(transform.position, new Vector2(m_HorizontalMovement * 10f, m_Rigidbody2D.velocity.y));
    }


    private void HorizontalInput()
    {
        m_HorizontalMovement = Input.GetAxisRaw("Horizontal") * m_MovementSpeed;
    }
    private void JumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_InputBufferJumpTimer = m_InputBufferJump;
        }

        if (m_InputBufferJumpTimer >= 0)
        {
            m_Jump = true;
        }
        else m_Jump = false;
    }

    private void HalfJumpInput()
    {
        if (Input.GetButtonUp("Jump") && m_HalfJumps)
        {
            if (m_Rigidbody2D.velocity.y > 2)
            {
                m_SmallJump = true;
            }
        }
    }

    private void DashInput()
    {
        if (Input.GetButtonDown("left ctrl") && m_DashCooldownTimer <= 0 && m_DashActive)
        {
            m_Dash = true;
            m_DashCooldownTimer = m_DashCooldown;
        }
    }
    private void FixedUpdate()
    {
        //Timers 
        m_DashCooldownTimer -= Time.deltaTime;
        HandleHangTime();
        HandleInputTimers();

        //Movement
        GroundCheck();
        Move(m_HorizontalMovement * Time.fixedDeltaTime, m_Jump, m_SmallJump);
        Jump(m_Jump);
        SmallJump(m_SmallJump);
        FallMultiplier();
        Dash(m_Dash);
    }



    private void Move(float move, bool jump, bool smallJump)
    {
        if (m_Grounded || m_AirControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            HandleFlip(move);
        }
    }


    private void Jump(bool jump)
    {
        if (m_Grounded && jump || jump && m_HangCounter > 0)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Jumpforce);
            m_Grounded = false;
            m_HangCounter = 0f;
            m_Jump = false;
        }
    }

    private void SmallJump(bool smallJump)
    {
        if (smallJump)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * 0.4f);
            m_Grounded = false;
            m_SmallJump = false;
        }
    }

    private void FallMultiplier()
    {
        if (m_Rigidbody2D.velocity.y < m_FallMulitplierStartVelocity)
        {
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_FallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Dash(bool dashActive)
    {
        if (dashActive)
        {
            if (m_DashTime <= 0)
            {
                m_Dash = false;
                m_FlipDisabled = false;
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_DashTime = m_StartDashTime;
            }
            else
            {

                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                m_FlipDisabled = true;
                m_DashTime -= Time.deltaTime;

                if (m_FacingRight)
                {
                    m_Rigidbody2D.velocity = Vector2.right * m_DashSpeed;
                }
                else if (!m_FacingRight)
                {
                    m_Rigidbody2D.velocity = Vector2.left * m_DashSpeed;
                }
            }
        }
    }



    private void GroundCheck()
    {
        m_Grounded = false;
        Collider2D[] groundCheck = GetGroundCheck();
        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (groundCheck[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }
    }

    private Collider2D[] GetGroundCheck()
    {
        return Physics2D.OverlapBoxAll(m_Collider2D.bounds.center, m_Collider2D.bounds.size, 0.0f, m_WhatIsGround);
    }

    private void HandleHangTime()
    {
        m_HangCounter -= Time.fixedDeltaTime;
        if (m_Grounded)
        {
            m_HangCounter = m_HangTime;
        }
    }


    private void HandleFlip(float move)
    {
        if (!m_FlipDisabled)
        {
            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void HandleInputTimers()
    {
        m_InputBufferJumpTimer -= Time.deltaTime;
    }

    
}

