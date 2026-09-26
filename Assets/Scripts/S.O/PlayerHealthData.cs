using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthData", menuName = "Scriptable Objects/PlayerHealthData")]
public class PlayerHealthData : ScriptableObject
{
    public float baseHealth=100f;
    public float baseEnergy = 30f;
    public float energyRecharge = 1f;



    [Header("Debuff Effect")]

    public float poisonousDuration = 3f;
    public float poisionousDamage = 5f;
    public float poisonousDamagePerSecond = 1f;

    public float burningDuration = 3f;
    public float burningDamage = 2f;
    public float burningDamagePerSecond = 0.5f;


}
