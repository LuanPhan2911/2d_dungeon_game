using UnityEngine;

[CreateAssetMenu(fileName = "Player Health", menuName = "Scriptable Objects/Player Health")]
public class PlayerHealthSO : ScriptableObject
{
    public float baseHealth=100f;
    public float baseEnergy = 40f;

    public float baseStamina = 100f;
    public float staminaRegenRate = 10f;

    public float staminaRegainDelay = 1f;


    public float energyNeedForBurstSkill = 40f;



    public float gainingEnergyFromNormalAttack = 1f;
    public float gainingEnergyFromElementalSkill = 15f;



    [Header("Effects")]

    public DebuffEffectSO posionEffectSO;
    public DebuffEffectSO burnEffectSO;





}
