using UnityEngine;


public class PlayerElementSO : ScriptableObject
{
    public ElementSO elementSO;
    public Sprite elementStoneSprite;




    [Header("Burst Skill")]
    public float burstCooldown=20f;
    public float burstEnergy=40f;

    public Sprite burstSprite;
    public string burstDescription;

    [Header("Resisatance")]
    public ElementalResistance[] resistances;

    public ElementalResistance bonusResistance;
    public float resistanceBonusDuration = 10f;
}








