using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    [SerializeField] private LayerMask whatIsGround;
    public Collider2D collider2d;

    protected override void Awake()
    {
        base.Awake();
        collider2d = GetComponentInParent<Collider2D>();
    }

    #region Check Functions


    public bool CheckIfGrounded()
    {
        Vector3 centerLowY = new Vector3(collider2d.bounds.center.x, (collider2d.bounds.center.y - (collider2d.bounds.size.y / 2)), 0);
        Vector3 sizeLowY = new Vector3(collider2d.bounds.size.x - 0.2f, 0.3f, 0);

        if (core.Movement.CurrentVelocity.y <= 3f)
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
}
