using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2D;
    [SerializeField] Camera MainCamera;
    CharacterController2D m_CharacterController2D;
    private Vector3 m_MousePosition;

    public float m_MaxAttackCooldown;
    private float m_AttackCooldown;
    public float m_MaxInputBufferAttack;
    private float m_InputBufferAttack;

    public float m_MaxDodgeCooldown;
    private float m_DodgeCooldown;
    public float m_MaxInputBufferDodge;
    private float m_InputBufferDodge;
    public float m_DodgeForce;


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_AttackCooldown = m_MaxAttackCooldown;
        m_DodgeCooldown = m_MaxDodgeCooldown;
    }
    void Update()
    {
        OnMouse1Pressed();
        OnMouse2Pressed();

        UppdateMousePosition();
    }
    private void FixedUpdate()
    {
        HandleCooldowns();
    }


    private void OnMouse1Pressed()
    {
        if (Input.GetButtonDown("Left click"))
        {
            m_InputBufferAttack = m_MaxInputBufferAttack;
        }

        if (m_InputBufferAttack >= 0)
        {
            TryAttack();
        }
    }

    private void OnMouse2Pressed()
    {
        if (Input.GetButtonDown("Right click"))
        {
            m_InputBufferDodge = m_MaxInputBufferDodge;
        }

        if (m_InputBufferDodge >= 0)
        {
            TryDodge();
        }
    }

    public void TryAttack()
    {
        if (m_AttackCooldown <= 0)
        {
            Attack();
        }
    }

    public void TryDodge()
    {
        if (m_DodgeCooldown <= 0)
        {
            Dodge();
        }
    }
    public void Attack()
    {
        m_AttackCooldown = m_MaxAttackCooldown;
    }

    public void Dodge()
    { 
        Debug.Log(m_MousePosition);

        Vector2 dodgeDirection = -(m_MousePosition - transform.position).normalized;
        Vector3 emptyVector3 = new Vector3(0, 0, 0);

        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, dodgeDirection * m_DodgeForce, ref emptyVector3, 0.01f);

        Debug.Log(dodgeDirection);


        m_DodgeCooldown = m_MaxDodgeCooldown;
    }

    private void HandleCooldowns()
    {
        if (m_InputBufferAttack > -1)
        {
            m_InputBufferAttack -= Time.deltaTime;
        }
        if (m_InputBufferDodge > -1)
        {
            m_InputBufferDodge -= Time.deltaTime;
        }


        if (m_AttackCooldown > -1)
        {
            m_AttackCooldown -= Time.deltaTime;
        }
        if (m_DodgeCooldown > -1)
        {
            m_DodgeCooldown -= Time.deltaTime;
        }
    }

    private void UppdateMousePosition()
    {
        m_MousePosition = MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));
        m_MousePosition.z = 0;
    }
}
