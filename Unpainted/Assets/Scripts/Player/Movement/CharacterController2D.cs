 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    #region Constants

    #region Movement
    [SerializeField] private float m_MovementSpeed = 40.0f;
    [Range(0, .7f)] [SerializeField] private float m_MovementSmoothing = .05f;
    #endregion

    [SerializeField] private float m_Jumpforce = 400.0f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private bool m_HalfJumps = false;
    [Range(0, .3f)] [SerializeField] private float m_HangTime = .1f;

    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;

    [SerializeField] private bool m_Dash = false;
    [SerializeField] private float m_DashSpeed = 100;

    #endregion

    private bool m_Grounded;
    private bool m_Jump = false;
    //Small jumps
    private bool m_SmallJump = false;

    //HangTime
    private float m_HangCounter = 0f;


    private float m_HorizontalMovement;


    private Rigidbody2D m_Rigidbody2D;
    private Collider2D m_Collider2D;
    private Vector3 m_Velocity = Vector3.zero;
    private bool m_FacingRight = true;

    private void Awake()
    {
        m_Collider2D = GetComponent<Collider2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Horizontal input 
        m_HorizontalMovement = Input.GetAxisRaw("Horizontal") * m_MovementSpeed;

        //Jump input 
        if (Input.GetButtonDown("Jump"))
        {
            m_Jump = true;
        }

        //Half jumps
        if (Input.GetButtonUp("Jump") && m_HalfJumps)
        {
            if (m_Rigidbody2D.velocity.y > 2)
            {
                m_SmallJump = true;
            }
        }
    }
    private void FixedUpdate()
    {
        //debug log
            //Hangtime
        if (m_Jump && m_HangCounter > 0 && !m_Grounded)
        {
            Debug.Log("hangTime jump");
        }
            //Smalljump
        if (m_SmallJump)
        {
            Debug.Log("Small Jump");
        }
            //Jump
        if (m_Grounded && m_Jump || m_Jump && m_HangCounter > 0)
        {
            Debug.Log("Jump = false");
        }

            //Groundcheck draw 
        //Debug.DrawRay()
        



        //Movement loop
        HandleHangTime();
        GroundCheck();
        Move(m_HorizontalMovement * Time.fixedDeltaTime, m_Jump, m_SmallJump);
    }


    private void Move(float move, bool jump, bool smallJump)
    {
        if (m_Grounded || m_AirControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            HandleFlip(move);
       
            //Jumps
            if (m_Grounded && jump || jump && m_HangCounter > 0)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Jumpforce);
                m_Grounded = false;
                m_HangCounter = 0f;
                m_Jump = false;
            }

            if (smallJump)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * 0.4f);
                m_Grounded = false;
                m_SmallJump = false;
            }
        }
    }





    private void GroundCheck()
    {
        m_Grounded = false;
        Collider2D[] groundCheck = Physics2D.OverlapBoxAll(m_Collider2D.bounds.center, m_Collider2D.bounds.size, 0.0f, m_WhatIsGround);
        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (groundCheck[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }
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
        if (move > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

