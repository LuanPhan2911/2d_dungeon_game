using System.Collections;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private AudioClip[] _walkingSounds;

   
    private Player _player;
    private PlayerCroching _playerCroching;
    private float _footStepSoundRate = 0.3f;
    private Coroutine _footStepCoroutine = null;


    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerCroching = GetComponent<PlayerCroching>();

    }
    public void HandleMoving()
    {
        _player.HorizontalInput = GameInputManager.Instance.GetHorizontalInput();

       _playerCroching.UpdateCrochWalk();
        UpdateRun();

        float horizontalVelocity = _player.CurrentVelocity.HorizontalVelocity;
        _player.HorizontalVelocity = _player.HorizontalInput * horizontalVelocity;

        if (_player.IsGrounded && Mathf.Abs(_player.HorizontalInput) > 0.1f && !_player.IsCrochWalking)
        {
            if(_footStepCoroutine== null)
            {
                _footStepCoroutine = StartCoroutine(PlayFootStepCoroutine());
            }
        }
     
    }

    public void CheckRun()
    {
        if (!_player.IsGrounded || _player.IsCroching)
        {
            _player.IsRunning = false;
            return;
        }
        if (GameInputManager.Instance.IsRunWasPressedThisFrame())
        {
            _player.IsRunning = true;
        }else if (GameInputManager.Instance.IsRunWasReleasedThisFrame())
        {
            _player.IsRunning= false;
        }
    }

    private void UpdateRun()
    {
        if (_player.IsCroching) return;

        if (_player.IsRunning && Mathf.Abs(_player.HorizontalInput) > 0.1f)
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
