using UnityEngine;


public class PlayerCroching : MonoBehaviour
{
    private Player _player;

    [SerializeField] private PlayerVelocity _crochWalkingVelocity;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void CheckCroching()
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

    public void CheckCrockWalking(float horizontalInput)
    {
        if (_player.IsCroching && Mathf.Abs(horizontalInput) > 0.1f)
        {
            _player.IsCrochWalking = true;
            _player.CurrentPlayerVelocity = _crochWalkingVelocity;  
        }
        else
        {
            _player.IsCrochWalking = false;
            _player.CurrentPlayerVelocity = _player.DefaultPlayerVelocity;
        }
    }
}
