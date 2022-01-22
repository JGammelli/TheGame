using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent
{

    public CombatData CombatData;
    private List<IDamagable> DetectedDamagables = new List<IDamagable>();


    #region AttackTrigger

    private void Start()
    {
        if (CombatData == null)
        {
            Debug.LogError("Missing CombatData for: " + transform.root.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AddToDetectedList(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveFromDetectedList(collision);
    }

    public void Attack()
    {
        foreach (IDamagable damagable in DetectedDamagables)
        {
            damagable.Damage(CombatData.damage);
        }
    }

    #endregion

    public void AddToDetectedList(Collider2D collision)
    {


        IDamagable damagable = collision.GetComponent<IDamagable>();  

        if (damagable != null)
        {
            DetectedDamagables.Add(damagable);

        }
    }

    public void RemoveFromDetectedList(Collider2D collision)
    {


        IDamagable damagable = collision.GetComponent<IDamagable>();



        if (damagable != null)
        {
            DetectedDamagables.Remove(damagable);
        }
    }
}
