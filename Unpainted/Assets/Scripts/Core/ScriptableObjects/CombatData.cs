using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCombatData", menuName = "Data/Combat Data/Base Data")]

public class CombatData : ScriptableObject
{
    [Header("Damage")]
    public float damage = 1;
}
