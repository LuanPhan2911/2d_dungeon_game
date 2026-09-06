using System.Collections;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private AudioClip[] _walkingSounds;


    private Player _player;
    private float _footStepSoundRate = 0.3f;
    private Coroutine _footStepCoroutine = null;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    public void HandleMoving()
    {
        if(_player.IsWallJumping|| _player.IsStopAction)
        {
            return;
        }

        UpdateVelocity();

        float horizontalVelocity = _player.CurrentVelocity.HorizontalVelocity;
        _player.Rb.linearVelocity = new Vector2(_player.HorizontalInput * horizontalVelocity, 
            _player.Rb.linearVelocityY);




        // handle play sound

        PlaySoundFX();
     
    }

    private void PlaySoundFX()
    {
        if (_player.IsCrouching || !_player.IsHorizontalMoving) return;

        if (_player.IsRunning)
        {
            // TODO: Play running sound
        }
        else
        {
            if (_footStepCoroutine == null)
            {
                _footStepCoroutine = StartCoroutine(PlayFootStepCoroutine());
            }
        }
    }


    private void UpdateVelocity()
    {
    
        if (_player.IsCrouching)
        {
            _player.CurrentVelocity = _player.CrouchWalkVelocity;

        } else if (_player.IsRunning)
        {
            _player.CurrentVelocity = _player.RunVelocity;
        }
        else
        {
            _player.CurrentVelocity = _player.WalkVelocity;
        }
    }

   
    private IEnumerator PlayFootStepCoroutine()
    {
        AudioManager.Instance.Play(_walkingSounds, transform.position);
        yield return new WaitForSeconds(_footStepSoundRate);
        _footStepCoroutine = null;

    }
}
