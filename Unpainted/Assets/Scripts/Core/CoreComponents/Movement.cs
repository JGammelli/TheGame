using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 workspace;


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
   //     if (rb.velocity.x >= playerData.movementVelocity)
   //     {
   //         rb.velocity = new Vector2(playerData.movementVelocity, rb.velocity.y);
   //     }
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
}
