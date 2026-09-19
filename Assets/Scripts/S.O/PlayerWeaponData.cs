using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponData", menuName = "Scriptable Objects/PlayerWeaponData")]
public class PlayerWeaponData : ScriptableObject
{
    public float attackCooldown=0.3f;
    public float attackDuration=0.1f;


    public int level1Damage=9;
    public int level2Damage=14;
    public int level3Damage=19;
}
