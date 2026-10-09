using UnityEngine;

[CreateAssetMenu(fileName = "Steel Player Element", menuName = "Player Element/Steel Player Element")]
public class SteelPlayerElementSO : PlayerElementSO
{
    public float burstDamage = 40f;

  

    [Header("Counter Attack")]
    public Sprite counterAttackSprite;
    public float counterAttackDuration = 20f;
    public float counterAttackRate = 0.2f;


    public float damageReductionRate = 0.2f;

    public Sprite damageReductionSprite;
    public float damageReductionDuration = 20f;
}