using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponData", menuName = "Scriptable Objects/PlayerWeaponData")]
public class PlayerWeaponData : ScriptableObject
{
    public float baseAttackCooldown=0.5f;

    
    public float attackDuration=0.1f;

    public float downAttackDuration = 0.05f;


    public int level1Damage=9;
    public int level2Damage=14;
    public int level3Damage=19;

    [Header("Input Buffer")]
    public float attackInputBuffer = 0.2f;

    [Header("Buff")]
    public float buffAttackSpeedMultiplier = 1.5f;
}
