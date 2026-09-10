using UnityEngine;


public class PlayerCrouching : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void HandleCrouching()
    {
        if (!_player.IsJumping|| _player.IsStopAction)
        {
            _player.IsCrouching = false;
            return;
        }

        if ( GameInputManager.Instance.PlayerCrouchAction.IsPressed())
        {
            _player.IsCrouching = true;
        }
        else 
        {
            _player.IsCrouching = false;
        }

        HandleCrouchWalking();
    }


    private void HandleCrouchWalking()
    {
        if(_player.IsCrouching && _player.IsHorizontalMoving)
        {
            _player.IsCrouchWalking = true;
        }
        else
        {
            _player.IsCrouchWalking = false;
        }
    }
    
}
