using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent
{
    public Collider2D attackCollider;

    private void Start()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    #region AttackTrigger

    public void Attack()
    {
        Debug.Log("Attack");
    }

    #endregion
}
