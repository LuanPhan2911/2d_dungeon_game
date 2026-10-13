using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Attack", menuName = "Scriptable Objects/Player Attack")]
public class PlayerAttackSO : ScriptableObject
{
    public float baseAttackRate=0.4f;
    public float baseNormalAttackDamage = 10f;
    public float baseChargeAttackDamage = 20f;
    public float chargeAttackTime = 1.5f;
    public float chargeAttackThreshold = 0.5f; // use for spawn charge attack vfx,
    // if press charge attack button >= threshold, spawn vfx

    public float lowPlungeAttackDamage = 14f;
    public float highPlungeAttackDamage = 18f;
    public float hightHeightThreshold = 4.5f;

    [Header("Input Buffer")]
    public float attackInputBuffer = 0.2f;

    [Header("Buff")]
    public float buffAttackSpeedMultiplier = 1.5f;


    public float baseCritRate = 0.1f;
    public float baseCritDamage = 0.5f;

    public float critRateIncreasement = 0.025f;


}


[Serializable]
public class ElementalResistance
{
    public ElementSO element;
    public float resistance=0.1f;
}


public class Damage
{
    public float Amount { get; set; }
    public ElementSO ElementSO { get; set; }
    public bool IsCrit { get; set; }
    public bool CanCrit { get; set; } = true;
    public bool IsFromEffect { get; set; }  // Thêm thuộc tính này để xác định nguồn gốc của sát thương

    public GameObject SourceAttackGameObject;


    



}