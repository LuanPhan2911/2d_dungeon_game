using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthData", menuName = "Scriptable Objects/PlayerHealthData")]
public class PlayerHealthData : ScriptableObject
{
    public float baseHealth=100f;
    public float baseEnergy = 40f;
    public float energyNeedForBurstSkill = 40f;



    public float gainingEnergyFromNormalAttack = 1f;
    public float gainingEnergyFromElementalSkill = 15f;



    [Header("Debuff Effect")]

    public float poisonousDuration = 3f;
    public float poisionousDamage = 5f;
    public float poisonousDamagePerSecond = 1f;

    public float burningDuration = 3f;
    public float burningDamage = 2f;
    public float burningDamagePerSecond = 0.5f;


}
