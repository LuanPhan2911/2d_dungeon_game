using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Water Player Element", menuName = "Player Element/Water Player Element")]


public class WaterPlayerElementSO : PlayerElementSO
{
    public float healRate = 0.2f;

    public HealPerDuration healPerDuration;



    public float allResistanceRate = 0.2f;
    public float allResistancesDuration = 10f;



}

[Serializable]
public class HealPerDuration
{
    public float rate = 0.025f;
    public float interval = 0.5f;
    public float duration = 4f;
}