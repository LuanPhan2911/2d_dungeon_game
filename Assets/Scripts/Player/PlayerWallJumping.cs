
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWallJumping : MonoBehaviour
{



    [SerializeField] private Transform _leftWallCheckPoint;
    [SerializeField] private Transform _rightWallCheckPoint;


    [SerializeField] private Vector2 _wallCheckSize= new Vector2(0.03f, 0.3f);


    private int _lastWallJumpDirection;



    private float _wallJumpStartTime;
  
 

    private Player _player;
    private void Awake()
    {
        _player = GetComponent<Player>();
    }


    public void HandleWallJump()
    {

        if (_player. IsWallJumping && Time.time - _wallJumpStartTime > _player.Data.wallJumpTime)
        {
          _player.IsWallJumping = false;
            _player.IsFalling = true;
        }

        if(CanWallJump() && _player.LastPressJumpTime > 0)
        {
            _player.IsWallJumping = true;
            _player.IsJumping = false;
            _player.IsJumpCut = false;
            _player.IsFalling = false;

            _wallJumpStartTime = Time.time;

            _lastWallJumpDirection= _player.LastOnRightWallTime >0 ? -1: 1;

            ExecuteWallJump();
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;


        Gizmos.DrawWireCube(_leftWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_rightWallCheckPoint.position, _wallCheckSize);



    }

    private void ExecuteWallJump()
    {
        _player.LastOnRightWallTime = 0;
        _player.LastPressJumpTime = 0;
        _player.LastOnLeftWallTime = 0;
        _player.LastOnRightWallTime = 0;

        #region Perform Wall Jump
        Vector2 force = new Vector2(_player.Data.wallJumpForce.x, _player.Data.wallJumpForce.y);

        force.x *= _lastWallJumpDirection;

        if(Mathf.Sign(_player.Rb.linearVelocityX)!= Mathf.Sign(force.x))
        {
            force.x -= _player.Rb.linearVelocityX;
        }
        if (_player.Rb.linearVelocityY < 0)
        {
            force.y-= _player.Rb.linearVelocityY;
        }

        _player.Rb.AddForce(force, ForceMode2D.Impulse);

        #endregion
    }

    public void CheckWallSlide()
    {
        if (CanWallSlide() && 
            ((_player.LastOnLeftWallTime>0 && _player.IsLeftMove) || (_player.LastOnRightWallTime>0 && _player.IsRightMove)))
        {

            _player.IsFalling = false;
            _player.IsWallSliding = true;
        }
        else
        {
            _player.IsWallSliding = false;
        }
    }
    public void HandleSlide()
    {
        
        if(_player.Rb.linearVelocityY > 0)
        {
            _player.Rb.AddForce(-_player.Rb.linearVelocityY * Vector2.up, ForceMode2D.Impulse );
        }

        float speedDiff = _player.Data.slideSpeed - _player.Rb.linearVelocityY;

        float movement = speedDiff * _player.Data.slideAccel;

        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDiff) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDiff) * (1 / Time.fixedDeltaTime));

        _player.Rb.AddForce(movement * Vector2.up, ForceMode2D.Force );

    }

    public void CheckWall()
    {
        if (_player.IsDashing || _player.IsJumping || _player.IsWallJumping) return;

        if ((CheckBox(_rightWallCheckPoint.position) && _player.IsFacingRight) ||
          (CheckBox(_leftWallCheckPoint.position) && !_player.IsFacingRight))
        {
           _player. LastOnRightWallTime = _player.Data.coyoteTime;
        }

        if ((CheckBox(_rightWallCheckPoint.position) && !_player.IsFacingRight) ||
            (CheckBox(_leftWallCheckPoint.position) && _player.IsFacingRight))
        {
          _player.  LastOnLeftWallTime = _player.Data.coyoteTime;
        }
      

       _player. LastOnWallTime = Mathf.Max(_player. LastOnLeftWallTime, _player. LastOnRightWallTime);

    }

    private bool CheckBox(Vector2 position)
    {
        return Physics2D.OverlapBox(position, _wallCheckSize, 0f, _player.WallLayerMask);
    }

    private bool CanWallSlide()
    {
        return _player. LastOnWallTime > 0 && _player.LastOnGroundTime<=0 && 
            !_player.IsJumping && !_player.IsWallJumping && !_player.IsDashing ;
    }

    public bool CanWallJumpCut()
    {
        return _player.IsWallJumping && _player.Rb.linearVelocityY > 0;
    }

    private bool CanWallJump()
    {
        return _player.LastOnWallTime > 0 && _player.LastOnGroundTime <= 0  &&
            !_player.IsWallJumping ;
    }
}
