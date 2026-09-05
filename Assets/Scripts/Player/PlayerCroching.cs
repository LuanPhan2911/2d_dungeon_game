using UnityEngine;


public class PlayerCroching : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void CheckCroch()
    {
        if (!_player.IsGrounded)
        {
            _player.IsCroching = false;
            return;
        }


        if ( GameInputManager.Instance.IsCrochWasPressedThisFrame())
        {
            
            _player.PlayerAnimation.SetTriggerStartCroch();
        }
        else if (GameInputManager.Instance.IsCrochWasReleasedThisFrame())
        {
            _player.PlayerAnimation.SetTriggerEndCroch();
            
        }
    }

    public void UpdateCrochWalk()
    {
        
        if (_player.IsCroching && Mathf.Abs(_player.HorizontalInput) > 0.1f)
        {
            _player.IsCrochWalking = true;
            _player.CurrentVelocity = _player.CrochWalkVelocity;  
        }
        else
        {
            _player.IsCrochWalking = false;
            _player.CurrentVelocity = _player.WalkVelocity;
        }
    }
}
