using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Attack", menuName = "Scriptable Objects/Player Attack")]
public class PlayerAttackSO : ScriptableObject
{
    public float baseAttackCooldown=0.4f;

    public int level1Damage=9;
    public int level2Damage=14;
    public int level3Damage=19;

    [Header("Input Buffer")]
    public float attackInputBuffer = 0.2f;

    [Header("Buff")]
    public float buffAttackSpeedMultiplier = 1.5f;


    public float baseCritRate = 0.1f;
    public float baseCritDamage = 0.5f;

    public float critRateIncreasement = 0.05f;


}


[Serializable]
public class ElementalResistance
{
    public ElementSO element;
    public float resistance=0.1f;
}
public class Damage
{
    public float amount { get; set; }
    public ElementSO element { get; set; }
    public bool isCrit { get; set; }

   
}