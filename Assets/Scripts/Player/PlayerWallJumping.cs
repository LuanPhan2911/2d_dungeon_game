
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWallJumping : MonoBehaviour
{

    public bool CanWallSlide = false;

    [SerializeField]  private Vector2 _wallJumpVelocity = new Vector2(6, 10);

    [SerializeField] private float _slidingVelocityY = 1f;

    [SerializeField] private Transform _wallCheckPoint;

    [SerializeField] private float _distanceCheck = 0.4f;



    [Header("Juice Mechanics")]
    // Grace period to jump after walking off a ledge(in seconds)
    [SerializeField] private float _coyoteTime = 0.15f;

    [SerializeField] private float _jumpBufferTime = 0.15f;

    [SerializeField] private float _wallJumpTime = 0.2f;

    [SerializeField] private float _clingWallDuration = 0.2f;
    private float _coyoteTimeCounter;

    // How early a player can press jump before landing (in seconds)
    private float _jumpBufferCounter;

    private float _wallJumpTimeCounter;


    private bool _wasTouchWallLastFrame;

    private float _clingCounter;

    private Player _player;
    private void Awake()
    {
        _player = GetComponent<Player>();
    }


    public void HandleWallJump()
    {
        if (_player.IsStopAction) return;

        // 1. COYOTE TIME LOGIC
        if (_player.IsWallSliding)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }


        // 2. JUMP BUFFER LOGIC
        if (GameInputManager.Instance.PlayerJumpAction.WasPressedThisFrame())
        {
          
            _jumpBufferCounter = _jumpBufferTime;
            
            _wallJumpTimeCounter = _wallJumpTime;
           
        }
        else
        {

            _jumpBufferCounter -= Time.deltaTime;
        }

       
     
        // 3. EXECUTE JUMP
        if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f )
        {

            _player.IsWallJumping = true;
            _player.Rb.linearVelocity = new Vector2(_player.FacingDirection * _wallJumpVelocity.x, _wallJumpVelocity.y); ;
            
            _coyoteTimeCounter = 0f;
            _jumpBufferCounter = 0f;

        }

        if (_player.IsWallJumping)
        {
            _wallJumpTimeCounter -= Time.deltaTime;
            if (_wallJumpTimeCounter < 0)
            {
                _player.IsWallJumping = false;
                _wallJumpTimeCounter = 0f;
            }
        }


        if (GameInputManager.Instance.PlayerJumpAction.WasReleasedThisFrame())
        {

            _coyoteTimeCounter = 0f;
            _wallJumpTimeCounter = 0f;
            _player.IsWallJumping = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 from = _wallCheckPoint.position;
        int direction = _player != null ? _player.LastHorizontalInput : 1;
        Vector3 to = from + Vector3.right * direction * _distanceCheck;

        Gizmos.DrawLine(from, to);



    }

    public void HandleWallSlide()
    {
        if (_player.IsGrounded || _player.IsCrouching || _player.IsWallJumping || _player.IsStopAction)
        {
            _clingCounter = 0f;
            _player.IsWallSliding = false;
            _wasTouchWallLastFrame = false;
            return;
        }
        



        if (CanWallSlide && _player.IsHorizontalMoving  )
        {

            if (!_wasTouchWallLastFrame)
            {
                // first time touch wall

                _clingCounter = _clingWallDuration;
                bool isFacingRight = _player.LastHorizontalInput == -1;
                _player.PlayerSprite.SetFacingRight(isFacingRight);
                _player.IsWallSliding = true;
            }



            if (_clingCounter > 0f)
            {
                _clingCounter -= Time.deltaTime;

                // reset velocity y, prevent slide up when cling
                _player.Rb.linearVelocity = new Vector2(_player.Rb.linearVelocityX, 0f);
             
            }
            else
            {
               // slide down
                _player.Rb.linearVelocity = new Vector2(_player.Rb.linearVelocityX, 
                    Mathf.Clamp(_player.Rb.linearVelocityY, -_slidingVelocityY, float.MaxValue));
               
            }


            _wasTouchWallLastFrame = CanWallSlide;
        }
        else
        {
            _player.IsWallSliding = false;
            _clingCounter = 0f;
            _wasTouchWallLastFrame = false;

        }

       

     
    }

    public void CheckWall()
    {
       
        Vector2 origin = (Vector2)_wallCheckPoint.position;
        CanWallSlide = Physics2D.Raycast(origin, Vector2.right * _player.LastHorizontalInput, _distanceCheck, _player.WallLayerMask);

    }
}
