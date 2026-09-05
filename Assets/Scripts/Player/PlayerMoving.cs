using System.Collections;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private AudioClip[] _walkingSounds;

   
    private Player _player;
    private PlayerCrouching _playerCrouching;
    private float _footStepSoundRate = 0.3f;
    private Coroutine _footStepCoroutine = null;


    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerCrouching = GetComponent<PlayerCrouching>();

    }
    public void HandleMoving()
    {
        _player.HorizontalInput = GameInputManager.Instance.GetHorizontalInput();

       _playerCrouching.UpdateVelocity();
        UpdateVelicity();

        float horizontalVelocity = _player.CurrentVelocity.HorizontalVelocity;
        _player.Rb.linearVelocity = new Vector2(_player.HorizontalInput * horizontalVelocity, 
            _player.Rb.linearVelocityY);

        if (_player.IsGrounded && _player.IsHorizontalMoving && !_player.IsCrouchWalking)
        {
            if(_footStepCoroutine== null)
            {
                _footStepCoroutine = StartCoroutine(PlayFootStepCoroutine());
            }
        }
     
    }

    public void CheckRun()
    {
        if (!_player.IsGrounded || _player.IsCrouching)
        {
            _player.IsRunning = false;
            return;
        }
        if (GameInputManager.Instance.PlayerRunAction.IsPressed())
        {
            _player.IsRunning = true;
        }
        else
        {
            _player.IsRunning = false;
        }
    }

    private void UpdateVelicity()
    {
        if (_player.IsCrouching) return;

        if (_player.IsRunning && _player.IsHorizontalMoving)
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
