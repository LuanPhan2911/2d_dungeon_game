using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectSO", menuName = "Scriptable Objects/StatusEffectSO")]


public class StatusEffectSO : ScriptableObject
{
    public string effectName;
    public ElementSO elementSO;
    public EffectType type;
    public GameObject effectUIPrefab;

    [Header("Settings")]
    public float maxDuration = 3.0f;
    public int maxStacks = 3;
    public float stackCooldown = 0.5f;
    public float tickInterval = 1.0f;

    [Header("Values")]
    public float valuePerTick; // Sát thương mỗi tick hoặc % làm chậm
    public float damageBonusPercent=0f; // Hệ số nhân sát thương
}
public enum EffectType { Poison, Burn }
