using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevelSO", menuName = "Scriptable Objects/PlayerLevelSO")]
public class PlayerLevelSO : ScriptableObject
{

    public int maxLevel = 30;
    public int breakthroughLevelNumber = 5;
    public int baseExpToLevelUp = 1000;
    public float baseHP = 100;
    public float baseATK = 25;
    public float baseDEF = 50;

    [Header("Level Up")]
    public float hpPerLevel = 5f;
    public float atkPerLevel = 1f;
    public float defPerLevel = 2f;

    [Header("Breakthrough Bonus")]
    public float hpBreakthrough = 45;
    public float atkBreakthrough = 10;
    public float defBreakthrough = 20;
}
