using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVelocity", menuName = "Scriptable Objects/PlayerVelocity")]

public class PlayerVelocity : ScriptableObject
{
    
    public PlayerStanding Standing;
    public float HorizontalVelocity;
    public float Acceleration;
}
public enum PlayerStanding
{
    Ground,
    Snow,
    Water,
}