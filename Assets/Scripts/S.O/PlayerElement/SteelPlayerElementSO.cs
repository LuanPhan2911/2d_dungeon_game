using UnityEngine;

[CreateAssetMenu(fileName = "Steel Player Element", menuName = "Player Element/Steel Player Element")]
public class SteelPlayerElementSO : PlayerElementSO
{
    [Header("Slash")]
    public float burstDamage = 40f;
    public float slashBurstDuration = 2f;
    public float slashBurstDistance = 10f;

  

    [Header("Counter Attack")]

    public float counterAttackDuration = 20f;
    public float counterAttackRate = 0.2f;

    [Header("Damage Reduction")]

    public float damageReductionRate = 0.2f;
    public float damageReductionDuration = 20f;
}