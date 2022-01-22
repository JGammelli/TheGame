using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummy : MonoBehaviour, IDamagable
{
    public void Damage(float amount)
    {
        Debug.Log(amount + "damage taken");
    }

    private void Awake()
    {
        
    }
}
