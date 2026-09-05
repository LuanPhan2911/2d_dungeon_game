using UnityEngine;


public class PlayerCrouching : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void CheckCrouch()
    {
        if (!_player.IsGrounded)
        {
            _player.IsCrouching = false;
            return;
        }


        if ( GameInputManager.Instance.PlayerCrouchAction.WasPressedThisFrame())
        {
            
            _player.PlayerAnimation.SetTriggerStartCrouch();
        }
        else if (GameInputManager.Instance.PlayerCrouchAction.WasReleasedThisFrame())
        {
            _player.PlayerAnimation.SetTriggerEndCrouch();
            
        }
    }

    public void UpdateVelocity()
    {
        
        if (_player.IsCrouching && _player.IsHorizontalMoving)
        {
            _player.IsCrouchWalking = true;
            _player.CurrentVelocity = _player.CrouchWalkVelocity;  
        }
        else
        {
            _player.IsCrouchWalking = false;
            _player.CurrentVelocity = _player.WalkVelocity;
        }
    }
}
