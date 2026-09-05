
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWallJumping : MonoBehaviour
{

    public bool CanWallSlide = false;
  

  
    [SerializeField]  private Vector2 _wallJumpVelocity = new Vector2(6, 10);

    [SerializeField] private Vector2 _wallCheckSize;
    [SerializeField] private Transform _wallCheckLeft;
    [SerializeField] private Transform _wallCheckRight;
    [SerializeField] private float _slidingVelocity = 2f;



    [Header("Juice Mechanics")]
    // Grace period to jump after walking off a ledge(in seconds)
    [SerializeField] private float _coyoteTime = 0.15f;

    [SerializeField] private float _jumpBufferTime = 0.15f;

    [SerializeField] private float _wallJumpTime = 0.2f;
    private float _coyoteTimeCounter;

    // How early a player can press jump before landing (in seconds)
    private float _jumpBufferCounter;

    private float _wallJumpTimeCounter;

   




    private Player _player;
    private void Awake()
    {
        _player = GetComponent<Player>();
    }


    public void HandleWallJump()
    {


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
            _player.IsWallJumping = true;
            _wallJumpTimeCounter = _wallJumpTime;
           
        }
        else
        {

            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_player.IsWallJumping)
        {
            _wallJumpTimeCounter -= Time.deltaTime;
            if(_wallJumpTimeCounter< 0)
            {
                _player.IsWallJumping = false;
                _wallJumpTimeCounter = 0f;
            }
        }
     
        // 3. EXECUTE JUMP
        if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f )
        {
        
            _player.Rb.linearVelocity = new Vector2(_player.DirectionX * _wallJumpVelocity.x, _wallJumpVelocity.y); ;
            
            _coyoteTimeCounter = 0f;
            _jumpBufferCounter = 0f;

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
        Gizmos.color = Color.black;

        Gizmos.DrawWireCube(_wallCheckLeft.position, _wallCheckSize);
        Gizmos.DrawWireCube(_wallCheckRight.position, _wallCheckSize);

    }

    public void HandleWallSlide()
    {
        if (CanWallSlide && _player.HorizontalInput !=0 && !_player.IsGrounded )
        {
            _player.IsWallSliding = true;
            float slidingVelocity = Mathf.Max(_player.Rb.linearVelocityY, -_slidingVelocity);

            _player.Rb.linearVelocity= new Vector2( _player.Rb.linearVelocityX, slidingVelocity);

            if(_player.DirectionX == 1)
            {
                _player.PlayerSprite.SetFacingRight(true);
            }else if (_player.DirectionX == -1)
            {
                _player.PlayerSprite.SetFacingRight(false);
            }

        }
        else
        {
            _player.IsWallSliding = false;
        }
    }

    public void CheckWall()
    {
        if (_player.IsGrounded) return;

        CanWallSlide = false;
       

        Collider2D leftCollider = Physics2D.OverlapBox(_wallCheckLeft.position, _wallCheckSize, 0f, _player.WallLayerMask);
        Collider2D rightCollider = Physics2D.OverlapBox(_wallCheckRight.position, _wallCheckSize, 0f, _player.WallLayerMask);

        if ( leftCollider != null || rightCollider != null)
        {
            CanWallSlide = true;
            // set wall jump direction
            if (leftCollider != null)
            {
  
                _player.DirectionX = 1;
            }
            else if (rightCollider != null)
            {
              
                _player.DirectionX = -1;
            }
        }
    }






}
