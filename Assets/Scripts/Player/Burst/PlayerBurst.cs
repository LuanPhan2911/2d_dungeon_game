using UnityEngine;

public abstract class PlayerBurst : MonoBehaviour
{
    [SerializeField] private DebuffEffectManager _debuffEffectManager;
    [SerializeField] private BuffEffectManager _buffEffectManager;


    public DebuffEffectManager DebuffEffectManager => _debuffEffectManager;
    public BuffEffectManager BuffEffectManager => _buffEffectManager;

    public abstract void TriggerBurst();
}
