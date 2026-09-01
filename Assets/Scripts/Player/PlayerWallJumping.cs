
using UnityEngine;

public class PlayerWallJumping : MonoBehaviour
{

    public float WallJumpDirection;
    public int JumpRemaining = 0;

    [SerializeField] private float _wallJumpDurationMax=0.25f;
    [SerializeField]  private Vector2 _wallJumpVelocity = new Vector2(6, 8);
    private float _wallJumpDuration=0;

    private Player _player;
    private void Awake()
    {
        _player = GetComponent<Player>();
    }


    public void HandleWallJump()
    {
        if (GameInputManager.Instance.PlayerJumpAction.WasPressedThisFrame() && JumpRemaining >0)
        {
            JumpRemaining--;
            _wallJumpDuration = 0f;
            _player.IsWallJumping= true;
          
        }
        if (GameInputManager.Instance.PlayerJumpAction.IsPressed() && _player.IsWallJumping)
        {
            _wallJumpDuration+=Time.deltaTime;
            bool isFirstJump = _player.JumpAvailable - JumpRemaining == 1;
            bool isAirJump = JumpRemaining == 0;
            float maxDuration = isFirstJump ? _wallJumpDurationMax : _player.MaxAirJumpDuration;
            if (_wallJumpDuration < maxDuration)
            {
              
                if (isFirstJump)
                {
                    _player.HorizontalVelocity = WallJumpDirection * _wallJumpVelocity.x;
                    _player.VerticalVelocity = _wallJumpVelocity.y;
                }else if (isAirJump)
                {
                    _player.VerticalVelocity = _player.AirJumpVelocity;
                    _player.PlayAirJumpFX();
                }
               
            }
            else
            {
                _player.IsWallJumping = false;
            }
        }

        if (GameInputManager.Instance.PlayerJumpAction.WasReleasedThisFrame())
        {
            _player.IsWallJumping = false;
        }
    }
   
   
}
