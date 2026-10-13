using UnityEngine;

public class ChargeAttackFX : MonoBehaviour
{
   [SerializeField] private ParticleSystem _particleSystem;

   

    public void Play()
    {
        if (_particleSystem!= null && !_particleSystem.isPlaying)
        {
            _particleSystem.Play();
        }
    }
    public void Stop()
    {
        if(_particleSystem!= null)
        {
            _particleSystem.Stop();
        }
        
    }
}
