
using UnityEngine;

public class PlayerWallJumping : MonoBehaviour
{

    public float WallJumpDirection;


    [SerializeField] private float _wallJumpDuration=0.25f;
    [SerializeField]  private Vector2 _wallJumpVelocity = new Vector2(6, 8);
    private float _jumpDuration=0;

    private Player _player;
    private void Awake()
    {
        _player = GetComponent<Player>();
    }


    public void HandleWallJump()
    {
      


        if (GameInputManager.Instance.PlayerJumpAction.WasPressedThisFrame() && _player.IsWallSliding )
        {

            Debug.Log("Wall jump start");
            _jumpDuration = 0f;
            _player.IsWallJumping= true;
          
        }
        
        if (GameInputManager.Instance.PlayerJumpAction.IsPressed() && _player.IsWallJumping)
        {
            _jumpDuration += Time.deltaTime;

            if (_jumpDuration < _wallJumpDuration)
            {

                _player.HorizontalVelocity = WallJumpDirection * _wallJumpVelocity.x;
                _player.VerticalVelocity = _wallJumpVelocity.y;

            }
            else
            {
                _player.IsWallJumping = false;
            }
        }
        
        
        if (GameInputManager.Instance.PlayerJumpAction.WasReleasedThisFrame())
        {
            Debug.Log("Wall jump release");
            _player.IsWallJumping = false;
        }
    }




   
   
}
