using UnityEngine;

[CreateAssetMenu(fileName = "Steel Player Element", menuName = "Player Element/Steel Player Element")]
public class PlayerElementSteelSO : PlayerElementSO
{
    public float damage = 40f;
    public float damageCounterRate = 0.2f;
    public float damageReductionRate = 0.2f;
}