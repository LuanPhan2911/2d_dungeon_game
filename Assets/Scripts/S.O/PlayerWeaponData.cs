using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponData", menuName = "Scriptable Objects/PlayerWeaponData")]
public class PlayerWeaponData : ScriptableObject
{
    public float baseAttackCooldown=0.4f;
    public float downAttackDuration = 0.05f;
    public float hitStopDuration = 0.05f;
    public float critHitStopDuration = 0.12f;
    public float hitStopTimeScale = 0.02f;


    public int level1Damage=9;
    public int level2Damage=14;
    public int level3Damage=19;

    [Header("Input Buffer")]
    public float attackInputBuffer = 0.2f;

    [Header("Buff")]
    public float buffAttackSpeedMultiplier = 1.5f;


    public float baseCritRate = 0.1f;
    public float baseCritDamge = 2f;

    public float critRateIncreasement = 0.05f;


    public ElementalType[] elementalTypes;

}

[Serializable]
public class ElementalType
{
    public string typeName;
    public Color elementalColor;

}
public class Damage
{
    public int amount { get; set; }
    public ElementalType elementalType { get; set; }
    public bool isCrit { get; set; }

   
}