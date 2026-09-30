using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class EnemySO : ScriptableObject
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
