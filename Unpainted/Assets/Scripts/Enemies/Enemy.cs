using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int attack;

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            die();
        }
    }
    public void die()
    {

    }

}
