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
    public void HandleMoving(float lerpAmount = 1)
    {
        if( _player.IsStopAction || _player.IsAttacking)
        {
            return;
        }

        //UpdateVelocity();
        float targetSpeed = _player.HorizontalInput * _player.Data.runMaxSpeed;

        targetSpeed = Mathf.Lerp(_player.Rb.linearVelocityX, targetSpeed, lerpAmount);
        float accelRate;
        if (_player.IsJumping)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _player.Data.runAccelAmount : _player.Data.runDeccelAmount;
        }
        else
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _player.Data.runAccelAmount * _player.Data.accelInAir :
                _player.Data.runDeccelAmount * _player.Data.deccelInAir;
        }



        float speedDiff = targetSpeed - _player.Rb.linearVelocityX;

        float movement = speedDiff * accelRate;

        _player.Rb.AddForce(movement * Vector2.right, ForceMode2D.Force);




        // handle play sound

        PlaySoundFX();
     
    }

    private void PlaySoundFX()
    {
        if (_player.IsCrouching || !_player.IsHorizontalMoving) return;

        if (_footStepCoroutine == null)
        {
            _footStepCoroutine = StartCoroutine(PlayFootStepCoroutine());
        }
    }

   
    private IEnumerator PlayFootStepCoroutine()
    {
        AudioManager.Instance.Play(_walkingSounds, transform.position);
        yield return new WaitForSeconds(_footStepSoundRate);
        _footStepCoroutine = null;

    }
}
