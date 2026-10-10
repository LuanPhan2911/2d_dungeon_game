using UnityEngine;

[CreateAssetMenu(fileName = "DebuffEffectSO", menuName = "Scriptable Objects/DebuffEffectSO")]


public class DebuffEffectSO : ScriptableObject
{
    public string effectName;
    public ElementSO elementSO;
    public Sprite sprite;
    public DebuffEffectType type;

    [Header("Settings")]
    public float maxDuration = 3.0f;
    public int maxStacks = 3;
    public float stackCooldown = 0.5f;
    public float tickInterval = 1.0f;

    [Header("Values")]
    public float baseValuePerTick; // Sát thương mỗi tick hoặc % làm chậm
}
public enum DebuffEffectType { Poison, Burn }
