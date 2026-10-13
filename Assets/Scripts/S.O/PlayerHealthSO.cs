using UnityEngine;

[CreateAssetMenu(fileName = "Player Health", menuName = "Scriptable Objects/Player Health")]
public class PlayerHealthSO : ScriptableObject
{
    public float baseHealth=100f;


    public int baseHealPotionCount = 2;
    public float baseHealPotionRate = 0.2f;
    public float healPotionTime = 1f;

    [Header("Debuff Effects")]
    public DebuffEffectSO[] debuffEffectSOArray;

    public DebuffEffectSO posionEffectSO;
    public DebuffEffectSO burnEffectSO;





}
