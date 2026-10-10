using UnityEngine;

[CreateAssetMenu(fileName = "Ground Player Element", menuName = "Player Element/Ground Player Element")]
public class GroundPlayerElementSO : PlayerElementSO
{
    [Header("Tornado")]
    public float tornadoDuration = 5f;
    public float tornadoMoveDistance = 10f;
    public float damageInterval = 0.5f;
    public float tornadoDamage = 5f;



    [Header("Buff")]

    public float moveSpeedRate = 0.2f;
    public float moveSpeedDuration = 10f;
    public float attackSpeedRate = 0.2f;
    public float attackSpeedDuration = 10f;


    public float debuffIncreasementEffectRate = 0.2f;
    public float debuffIncreasementEffectDuration = 10f;

}