using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Attack", menuName = "Scriptable Objects/Player Attack")]
public class PlayerAttackSO : ScriptableObject
{
    public float baseAttackRate=0.4f;

    public float baseNormalAttackDamage = 10f;

    public float baseChargeAttackDamage = 20f;
    public float chargeAttackHoldTime = 1.5f;
    public float chargeAttackStaminaCost = 20f;


    public float baseLowPlungeAttackDamage = 12f;
    public float baseHighPlungeAttackDamage = 18f;
    public float highHeightThreshold = 4.5f;

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

public enum DamageType
{
    NormalAttack,
    ChargeAttack,
    PlungeAttack,
    ElementalSkillAttack,
    BurstAttack,
}
public class Damage
{
    public float amount { get; set; }
    public ElementSO element { get; set; }
    public bool isCrit { get; set; }

   
    public DamageType type;


}