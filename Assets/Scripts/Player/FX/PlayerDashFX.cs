using UnityEngine;

public class PlayerDashFX : MonoBehaviour
{


    [SerializeField] private TrailRenderer[] _trailRenderers;
    public void PlayDashFX(bool isEmitting)
    {
        foreach(var _trail in _trailRenderers)
        {
            _trail.emitting = isEmitting;
        }
    }
}
