using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent
{
    public Collider2D attackCollider;
    private List<IDamagable> detectedDamagables = new List<IDamagable>();

    private void Start()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    #region AttackTrigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("On trigger enter");

        AddToDetectedList(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveFromDetectedList(collision);
    }
    public void Attack()
    {
    }

    #endregion

    public void AddToDetectedList(Collider2D collision)
    {


        IDamagable damagable = collision.GetComponent<IDamagable>();

        if (damagable != null)
        {
            detectedDamagables.Add(damagable);
        }
    }

    public void RemoveFromDetectedList(Collider2D collision)
    {


        IDamagable damagable = collision.GetComponent<IDamagable>();

        if (damagable != null)
        {
            detectedDamagables.Remove(damagable);
        }
    }
}
