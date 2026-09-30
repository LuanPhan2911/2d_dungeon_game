using UnityEngine;

[CreateAssetMenu(fileName = "Fire Player Element", menuName = "Player Element/Fire Player Element")]
public class PlayerElementFireSO : PlayerElementSO
{
    public float damage = 40f;

    public float burningEffectBonusDamageRate = 0.2f;

}