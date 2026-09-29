using UnityEngine;


public class PlayerElementData : ScriptableObject
{
    public ElementData element;


    [Header("Elemental Skill")]
    public float baseElementalSkillCooldown = 10f;
    public float baseElementalSkillDamage = 20f;

    [Header("Burst Skill")]
    public float infusedElementToWeaponDuration = 8.5f;
    public float burstSkillPressedThreshhold = 0.5f;
    public float burstCooldown;
    public float burstEnergy;

    public float skillInputBuffer = 0.2f;
}

[CreateAssetMenu(fileName = "SteelPlayerElementData", menuName = "Player_Element/SteelPlayerElementData")]
public class SteelPlayerElementData: PlayerElementData
{
    public float damage = 40f;
    public float damageCounterRate = 0.2f;
    public float damageReductionRate = 0.2f;
}


[CreateAssetMenu(fileName = "GrassPlayerElementData", menuName = "Player_Element/GrassPlayerElementData")]
public class GrassPlayerElementData : PlayerElementData
{
    public float damage = 10f;

    public float elementalResistanceReductionRate = 0.4f;

}
[CreateAssetMenu(fileName = "WaterPlayerElementData", menuName = "Player_Element/WaterPlayerElementData")]
public class WaterPlayerElementData : PlayerElementData
{
    public float healthHealRate = 0.2f;

}
[CreateAssetMenu(fileName = "FirePlayerElementData", menuName = "Player_Element/FirePlayerElementData")]
public class FirePlayerElementData : PlayerElementData
{
    public float damage = 40f;

    public float burningEffectBonusDamageRate = 0.2f;

}
[CreateAssetMenu(fileName = "GroundPlayerElementData", menuName = "Player_Element/GroundPlayerElementData")]
public class GroundPlayerElementData : PlayerElementData
{
    public float shieldRate = 0.2f;
    public float debuffEffectIncreasementRate = 0.5f;

}
