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

    public void UpdateVelocity()
    {
        
        if (_player.IsCroching && _player.IsHorizontalMoving)
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
