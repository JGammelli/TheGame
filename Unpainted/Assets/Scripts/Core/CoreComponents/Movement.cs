using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }

    [SerializeField] private float maxMovementX;

    private Vector2 workspace;


    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();

        rb = GetComponentInParent<Rigidbody2D>();

        FacingDirection = 1;
    }

    #endregion

    #region Uppdates
    public void LogicUppdate()
    {
        CurrentVelocity = rb.velocity;
    }

    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocitySmooth(float forceX, float forceY)
    {
        workspace = new Vector2(forceX, forceY);
        rb.AddForce(workspace);
        if (rb.velocity.x >= maxMovementX)
        {
            rb.velocity = new Vector2(maxMovementX, rb.velocity.y);
        }
        CurrentVelocity = rb.velocity;
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

    #region Flips
    private void Flip()
    {
        FacingDirection *= -1;
        rb.transform.Rotate(0.0f, 180.0f, 0.0f);
        Debug.Log("flipped");
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void CheckIfShouldFlipMousePos(Vector2 mousePos)
    {
        if (mousePos.x != 0 && Mathf.Round(mousePos.x) != FacingDirection)
        {
            Flip();
        }
    }

    #endregion
}
