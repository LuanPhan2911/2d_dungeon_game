using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class EnemySO : ScriptableObject
{
    public int maxHealth;
    public float flashDuration;
    public ElementSO element;

    [Header("Recoil")]
    public bool CanRecoil;
    public float recoilForce;
    public float recoilDuration;



    [Header("Attack")]
    public float baseDamage = 5f;

    [Header("Resisatance")]
    public ElementalResistance[] resistances;
    [Header("Debuff Effects")]
    public DebuffEffectSO[] debuffEffectSOArray;

}
