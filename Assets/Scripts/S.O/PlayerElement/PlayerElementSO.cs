using UnityEngine;


public class PlayerElementSO : ScriptableObject
{
    public ElementSO element;




    [Header("Burst Skill")]
    public float burstCooldown=20f;
    public float burstEnergy=40f;

    public float skillInputBuffer = 0.2f;

    [Header("Resisatance")]
    public ElementalResistance[] resistances;

    public ElementalResistance resistanceBonus;
    public float resistanceBonusDuration = 10f;
}








