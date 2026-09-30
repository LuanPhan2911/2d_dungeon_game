using UnityEngine;


public class PlayerElementSO : ScriptableObject
{
    public ElementSO element;




    [Header("Burst Skill")]
    public float infusedElementToWeaponDuration = 8.5f;
    public float burstSkillPressedThreshhold = 0.5f;
    public float burstCooldown=20f;
    public float burstEnergy=40f;

    public float skillInputBuffer = 0.2f;
}








