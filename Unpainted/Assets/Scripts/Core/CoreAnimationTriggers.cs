using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreAnimationTriggers : MonoBehaviour
{

    private Core core;
    private void Awake()
    {
        core = GetComponentInChildren<Core>();
    }



    private void CoreAnimationAttackTrigger()
    {
        core.Combat.Attack();
        Debug.Log("attacked");
    } 


}
