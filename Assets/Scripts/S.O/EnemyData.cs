using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int maxHealth;

    [Header("Recoil")]
    public bool CanRecoil;
    public float recoilForce;
    public float recoilDuration;

    public float flashDuration;
    [Header("Resisatance")]
    public ElementalResistance[] resistances;
   
}
