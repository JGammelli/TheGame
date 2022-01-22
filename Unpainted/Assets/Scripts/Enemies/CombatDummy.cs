using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummy : MonoBehaviour, IDamagable
{
    SpriteRenderer spriteRenderer;
    public void Damage(float amount)
    {
        Debug.Log(amount + "damage taken");

        spriteRenderer.material.SetFloat("_FlashAmount", 1);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
